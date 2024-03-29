﻿using UnityEngine;
using Zenject;

namespace Banchou.FSM {
    public class InputMovement : FSMBehaviour {
        [SerializeField, Tooltip("How quickly, in units per second, the object moves along its motion vector")]
        private float _movementSpeed = 8f;

        [SerializeField, Tooltip("Whether or not the object will rotate towards its motion vector")]
        private bool _rotateToInput = true;

        [SerializeField, Tooltip("How quickly, in degrees per second, the Object will rotate to face its motion vector")]
        private float _rotationSpeed = 10f;

        [SerializeField, Tooltip("How long, in seconds, the Object will face a direction before it rotates towards its motion vector")]
        private float _flipDelay = 0f;
        
        [Header("Animation Parameters")]
        [SerializeField, Tooltip("Animation parameter to write movement speed")]
        private string _movementSpeedOut = string.Empty;
        [SerializeField] private string _lateralVelocityOut = string.Empty;
        [SerializeField] private string _distalVelocityOut = string.Empty;

        [Inject] private Part.IMovementInput _input = null;
        [Inject] private Part.IMotor _motor = null;
        [Inject] private Part.Orientation _orientation = null;
        [Inject] private Animator _animator = null;
        private int _speedOut, _lateralOut, _distalOut;

        // The object's final facing unit vector angle
        private Vector3 _faceDirection;
        private float _flipTimer = 0f;

        private void OnEnable() {
            _speedOut = Animator.StringToHash(_movementSpeedOut);
            _lateralOut = Animator.StringToHash(_lateralVelocityOut);
            _distalOut = Animator.StringToHash(_distalVelocityOut);
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _faceDirection = _orientation.transform.forward;
            _flipTimer = 0f;
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            var direction = _input.MovementDirection;
            var velocity = _movementSpeed * direction;
            _motor.Move(velocity * Time.fixedDeltaTime);

            if (_rotateToInput && _input.RotateToMovement) {
                if (direction != Vector3.zero) {
                    // If the movement direction is different enough from the facing direction,
                    // remain facing in the current direction for a short time. Allows the player to
                    // more easily execute Pull Attacks
                    var faceMotionDot = Vector3.Dot(direction, _faceDirection);
                    if (faceMotionDot <= -0.01f && _flipTimer < _flipDelay) {
                        _flipTimer += Time.fixedDeltaTime;
                    } else {
                        _faceDirection = direction.normalized;
                        _flipTimer = 0f;
                    }
                }

                _orientation.transform.rotation = Quaternion.RotateTowards(
                    _orientation.transform.rotation,
                    Quaternion.LookRotation(_faceDirection.normalized),
                    _rotationSpeed * Time.fixedDeltaTime
                );
            }

            // Write to output variable
            if (!string.IsNullOrWhiteSpace(_movementSpeedOut)) {
                _animator.SetFloat(_speedOut, velocity.magnitude);
                _animator.SetFloat(_lateralOut, Vector3.Dot(velocity, _orientation.transform.right));
                _animator.SetFloat(_distalOut, Vector3.Dot(velocity, _orientation.transform.forward));
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            // Snap to the facing direction on state exit.
            // Helps face the character in the intended direction when jumping mid-turn.
            if (_rotateToInput && _input.RotateToMovement) {
                _orientation.transform.rotation = Quaternion.LookRotation(_faceDirection.normalized);
            }
        }
    }
}
