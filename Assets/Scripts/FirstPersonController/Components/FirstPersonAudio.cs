using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace FirstPersonController.Components
{
    public class FirstPersonAudio : MonoBehaviour
    {
        public FirstPersonMovement character;
        public GroundCheck groundCheck;

        [Header("Attack Sounds")]
        public AudioSource attackAudio;
        public AudioClip swordSwing;
        public AudioClip hitSound;
        public GameObject hitEffect;
        
        [Header("Step")]
        public AudioSource stepAudio;
        public AudioSource runningAudio;
        [Tooltip("Minimum velocity for moving audio to play")]
        public float velocityThreshold = .01f;
        Vector2 _lastCharacterPosition;
        Vector2 CurrentCharacterPosition => new Vector2(character.transform.position.x, character.transform.position.z);

        [Header("Landing")]
        public AudioSource landingAudio;
        [FormerlySerializedAs("landingSFX")] public AudioClip[] landingSfx;

        [Header("Jump")]
        public Jump jump;
        public AudioSource jumpAudio;
        [FormerlySerializedAs("jumpSFX")] public AudioClip[] jumpSfx;

        [Header("Crouch")]
        public Crouch crouch;
        public AudioSource crouchStartAudio, crouchedAudio, crouchEndAudio;
        [FormerlySerializedAs("crouchStartSFX")] public AudioClip[] crouchStartSfx;
        [FormerlySerializedAs("crouchEndSFX")] public AudioClip[] crouchEndSfx;

        AudioSource[] MovingAudios => new[] { stepAudio, runningAudio, crouchedAudio };


        void Reset()
        {
            // Setup stuff.
            character = GetComponentInParent<FirstPersonMovement>();
            groundCheck = (transform.parent ?? transform).GetComponentInChildren<GroundCheck>();
            stepAudio = GetOrCreateAudioSource("Step Audio");
            runningAudio = GetOrCreateAudioSource("Running Audio");
            landingAudio = GetOrCreateAudioSource("Landing Audio");
            attackAudio = GetOrCreateAudioSource("Attack Audio");

            // Setup jump audio.
            jump = GetComponentInParent<Jump>();
            if (jump)
            {
                jumpAudio = GetOrCreateAudioSource("Jump audio");
            }

            // Setup crouch audio.
            crouch = GetComponentInParent<Crouch>();
            if (crouch)
            {
                crouchStartAudio = GetOrCreateAudioSource("Crouch Start Audio");
                crouchStartAudio = GetOrCreateAudioSource("Crouched Audio");
                crouchStartAudio = GetOrCreateAudioSource("Crouch End Audio");
            }
        }

        void OnEnable() => SubscribeToEvents();

        void OnDisable() => UnsubscribeToEvents();

        void FixedUpdate()
        {
            // Play moving audio if the character is moving and on the ground.
            float velocity = Vector3.Distance(CurrentCharacterPosition, _lastCharacterPosition);
            if (velocity >= velocityThreshold && groundCheck && groundCheck.isGrounded)
            {
                if (crouch && crouch.IsCrouched)
                {
                    SetPlayingMovingAudio(crouchedAudio);
                }
                else if (character.IsRunning)
                {
                    SetPlayingMovingAudio(runningAudio);
                }
                else
                {
                    SetPlayingMovingAudio(stepAudio);
                }
            }
            else
            {
                SetPlayingMovingAudio(null);
            }

            // Remember lastCharacterPosition.
            _lastCharacterPosition = CurrentCharacterPosition;
        }


        /// <summary>
        /// Pause all MovingAudios and enforce play on audioToPlay.
        /// </summary>
        /// <param name="audioToPlay">Audio that should be playing.</param>
        void SetPlayingMovingAudio(AudioSource audioToPlay)
        {
            // Pause all MovingAudios.
            foreach (var audioSource in MovingAudios.Where(audioSource => audioSource != audioToPlay && audioSource))
            {
                audioSource.Pause();
            }

            // Play audioToPlay if it was not playing.
            if (audioToPlay && !audioToPlay.isPlaying)
            {
                audioToPlay.Play();
            }
        }

        #region Play instant-related audios.
        void PlayLandingAudio() => PlayRandomClip(landingAudio, landingSfx);
        void PlayJumpAudio() => PlayRandomClip(jumpAudio, jumpSfx);
        void PlayCrouchStartAudio() => PlayRandomClip(crouchStartAudio, crouchStartSfx);
        void PlayCrouchEndAudio() => PlayRandomClip(crouchEndAudio, crouchEndSfx);
        #endregion

        #region Subscribe/unsubscribe to events.
        void SubscribeToEvents()
        {
            // PlayLandingAudio when Grounded.
            groundCheck.Grounded += PlayLandingAudio;

            // PlayJumpAudio when Jumped.
            if (jump)
            {
                jump.Jumped += PlayJumpAudio;
            }

            // Play crouch audio on crouch start/end.
            if (crouch)
            {
                crouch.CrouchStart += PlayCrouchStartAudio;
                crouch.CrouchEnd += PlayCrouchEndAudio;
            }
        }

        void UnsubscribeToEvents()
        {
            // Undo PlayLandingAudio when Grounded.
            groundCheck.Grounded -= PlayLandingAudio;

            // Undo PlayJumpAudio when Jumped.
            if (jump)
            {
                jump.Jumped -= PlayJumpAudio;
            }

            // Undo play crouch audio on crouch start/end.
            if (crouch)
            {
                crouch.CrouchStart -= PlayCrouchStartAudio;
                crouch.CrouchEnd -= PlayCrouchEndAudio;
            }
        }
        #endregion

        #region Utility.
        /// <summary>
        /// Get an existing AudioSource from a name or create one if it was not found.
        /// </summary>
        /// <param name="name">Name of the AudioSource to search for.</param>
        /// <returns>The created AudioSource.</returns>
        AudioSource GetOrCreateAudioSource(string name)
        {
            // Try to get the audiosource.
            AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
            if (result)
                return result;

            // Audiosource does not exist, create it.
            result = new GameObject(name).AddComponent<AudioSource>();
            result.spatialBlend = 1;
            result.playOnAwake = false;
            result.transform.SetParent(transform, false);
            return result;
        }

        static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
        {
            if (!audio || clips.Length <= 0)
                return;

            // Get a random clip. If possible, make sure that it's not the same as the clip that is already on the audiosource.
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            if (clips.Length > 1)
                while (clip == audio.clip)
                    clip = clips[Random.Range(0, clips.Length)];

            // Play the clip.
            audio.clip = clip;
            audio.Play();
        }
        #endregion 
        
        #region Attack-related audio
        public void PlayAttackSound(bool isFirstAttack)
        {
            attackAudio.pitch = Random.Range(0.9f, 1.1f);
            attackAudio.PlayOneShot(swordSwing);
        }

        public void PlayHitSound(Vector3 pos)
        {
            attackAudio.pitch = 1;
            attackAudio.PlayOneShot(hitSound);
            
            Vector3 adjustedPos = pos + new Vector3(0, 0, -0.5f);
            GameObject go = Instantiate(hitEffect, adjustedPos, Quaternion.identity);
            go.transform.localScale *= 10;
            Destroy(go, 20);
        }
        #endregion
    }
}
