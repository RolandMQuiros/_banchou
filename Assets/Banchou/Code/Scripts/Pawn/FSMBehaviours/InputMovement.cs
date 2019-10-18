using UnityEngine;

namespace Banchou {
    public class InputMovement : FSMBehaviour {
        [SerializeField] private float _movementSpeed = 8f;
        [SerializeField] private bool _rotateToInput = true;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _flipDelay = 0f;
        [Header("Animation Parameters")]
        [SerializeField] private string _movementSpeedOut = string.Empty;

        private Part.IMovementInput _input;
        private Part.IMotor _motor;
        private Transform _orientation;
        private Animator _animator;
        private int _speedOut;

        private Vector3 _faceDirection;
        private float _flipTimer = 0f;

        public override void Inject(Animator stateMachine) {
            _input         = stateMachine.GetComponentInChildren<Part.IMovementInput>();
            _motor         = stateMachine.GetComponentInChildren<Part.IMotor>();
            _orientation   = stateMachine.GetComponentInChildren<Part.Orientation>().transform;
            _animator      = stateMachine.GetComponentInChildren<Animator>();

            _speedOut = Animator.StringToHash(_movementSpeedOut);
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _faceDirection = _orientation.forward;
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

                _orientation.rotation = Quaternion.RotateTowards(
                    _orientation.rotation,
                    Quaternion.LookRotation(_faceDirection.normalized),
                    _rotationSpeed * Time.fixedDeltaTime
                );
            }

            if (!string.IsNullOrWhiteSpace(_movementSpeedOut)) {
                var relativeDirection = Mathf.Sign(
                    Vector3.Dot(_faceDirection, _orientation.forward)
                );
                _animator.SetFloat(_speedOut, velocity.magnitude * relativeDirection);
            }
        }
    }
}
