using FirstPersonController.Components;
using UnityEngine;

namespace FirstPersonController
{
    
    public class AttackManager : MonoBehaviour
    {
        
        
        private PlayerInput _playerInput;
        private PlayerInput.MainActions _input;
        private Animator _animator;
        private Vector3 _playerVelocity;
        private Rigidbody _rigidbody;
        
        [Header("Attacking")]
        public float attackDistance = 3f;
        public float attackDelay = 0.4f;
        public float attackSpeed = 1f;
        public int attackDamage = 1;
        public LayerMask attackLayer;
        
        private bool _attacking;
        private bool _readyToAttack = true;
        private int _attackCount;

        private Camera _cam;
        
        // ---------- //
        // ANIMATIONS //
        // ---------- //

        public const string Idle = "Idle";
        public const string Walk = "Walk";
        public const string Attack1 = "Attack 1";
        public const string Attack2 = "Attack 2";
        
        private string _currentAnimationState;


        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _cam = GetComponentInChildren<Camera>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerInput = new PlayerInput();
            _input = _playerInput.Main;
            AssignInputs();
        }
        
        void Update()
        {
            _playerVelocity = _rigidbody.velocity;
            SetAnimations();
        }
        
        void OnEnable() 
        { _input.Enable(); }

        void OnDisable()
        { _input.Disable(); }
        
        void SetAnimations()
        {
            if (!_attacking)
            {
                if (Mathf.Abs(_playerVelocity.x) < 0.1f && Mathf.Abs(_playerVelocity.z) < 0.1f)
                {
                    ChangeAnimationState(Idle);
                }
                else
                {
                    ChangeAnimationState(Walk);
                }
            }
        }

        
        void AssignInputs()
        {
            _input.Attack.started += ctx => Attack();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Attack()
        {
            if (!_readyToAttack || _attacking ) return;

            _readyToAttack = false;
            _attacking = true;

            // Виклик затримки для удару
            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast), attackDelay);
            
            FindObjectOfType<FirstPersonAudio>().PlayAttackSound(_attackCount == 0);


            if (_attackCount == 0)
            {
                ChangeAnimationState(Attack1);
                _attackCount++;
            }
            else
            {
                ChangeAnimationState(Attack2);
                _attackCount = 0;
            }
        }
        
        void ResetAttack()
        {
            _attacking = false;
            _readyToAttack = true;
        }

        void AttackRaycast()
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out var hit, attackDistance, attackLayer))
            {
                HitTarget(hit.point);

                if (hit.transform.TryGetComponent<Enemy>(out Enemy T))
                {
                    T.TakeDamage(attackDamage);
                }
            }
        }

        void HitTarget(Vector3 pos)
        {
            FindObjectOfType<FirstPersonAudio>().PlayHitSound(pos);
        }

        void ChangeAnimationState(string newState)
        {
            if (_currentAnimationState == newState) return;

            // Плавний перехід між станами анімації
            _currentAnimationState = newState;
            _animator.CrossFadeInFixedTime(_currentAnimationState, 0.2f);
        }
    }
}
