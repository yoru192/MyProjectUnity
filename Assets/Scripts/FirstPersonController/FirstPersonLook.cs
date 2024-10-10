using UnityEngine;

namespace FirstPersonController
{
    public class FirstPersonLook : MonoBehaviour
    {
        [SerializeField]
        Transform character;
        public bool canRotate = true;
        public float sensitivity = 2;
        public float smoothing = 1.5f;

        Vector2 _velocity;
        Vector2 _frameVelocity;


        void Reset()
        {
            // Get the character from the FirstPersonMovement in parents.
            character = GetComponentInParent<FirstPersonMovement>().transform;
        }

        void Start()
        {
            // Lock the mouse cursor to the game screen.
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (canRotate)
            {
                // Get smooth velocity.
                Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
                _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1 / smoothing);
                _velocity += _frameVelocity;
                _velocity.y = Mathf.Clamp(_velocity.y, -90, 90);

                // Rotate camera up-down and controller left-right from velocity.
                transform.localRotation = Quaternion.AngleAxis(-_velocity.y, Vector3.right);
                character.localRotation = Quaternion.AngleAxis(_velocity.x, Vector3.up);
            }
        }
    }
}
