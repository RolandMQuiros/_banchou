using UnityEngine;
using Zenject;

namespace Banchou {
    public class InputMovement : FSMBehaviour {
        [SerializeField] private float _movementSpeed = 8f;
        [SerializeField] private bool _rotateToInput = true;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _flipDelay = 0f;
        [Header("Animation Parameters")]
        [SerializeField] private string _movementSpeedOut = string.Empty;

        [Inject] private Part.IMovementInput _input = null;
        [Inject] private Part.IMotor _motor = null;
        [Inject] private Part.Orientation _orientation = null;
        [Inject] private Animator _animator = null;
        private int _speedOut;

        private Vector3 _faceDirection;
        private float _flipTimer = 0f;

        private void OnEnable() {
            _speedOut = Animator.StringToHash(_movementSpeedOut);
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _faceDirection = _orientation.transform.forward;
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            var direction = _input.MovementDirection;
            var velocity = _movementSpeed * direction;
            _motor.Move(velocity * Time.fixedDeltaTime);

            if (_rotateToInput && _input.RotateToMovement) {
                if (direction != Vector3.zero) {
                    if (Vector3.Dot(direction, _faceDirection) < 0f && _flipTimer < _flipDelay) {
                        _flipTimer += Time.fixedDeltaTime;
                    } else {
                        _faceDirection = direction;
                        _flipTimer = 0f;
                    }
                }

                _orientation.transform.rotation = Quaternion.RotateTowards(
                    _orientation.transform.rotation,
                    Quaternion.LookRotation(_faceDirection.normalized),
                    _rotationSpeed * Time.fixedDeltaTime
                );
            }

            if (!string.IsNullOrWhiteSpace(_movementSpeedOut)) {
                var relativeDirection = Mathf.Sign(
                    Vector3.Dot(_faceDirection, _orientation.transform.forward)
                );
                _animator.SetFloat(_speedOut, velocity.magnitude * relativeDirection);
            }
        }
    }
}
