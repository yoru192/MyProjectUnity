﻿using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonController
{
    public class FirstPersonMovement : MonoBehaviour
    {
        public float speed = 5;

        [Header("Running")]
        public bool playerCanMove = true;
        public bool canRun = true;
        public bool IsRunning { get; private set; }
        public float runSpeed = 9;
        public KeyCode runningKey = KeyCode.LeftShift;

        Rigidbody _rigidbody;
        /// <summary> Functions to override movement speed. Will use the last added override. </summary>
        public readonly List<System.Func<float>> SpeedOverrides = new List<System.Func<float>>();



        void Awake()
        {
            // Get the rigidbody on this.
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (playerCanMove)
            {
                // Update IsRunning from input.
                IsRunning = canRun && Input.GetKey(runningKey);

                // Get targetMovingSpeed.
                float targetMovingSpeed = IsRunning ? runSpeed : speed;
                if (SpeedOverrides.Count > 0)
                {
                    targetMovingSpeed = SpeedOverrides[^1]();
                }

                // Get targetVelocity from input.
                Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

                // Apply movement.
                _rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.y);
            }
            
            
        }

    }
}