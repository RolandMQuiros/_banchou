using System.Linq;
using UnityEngine;
using UniRx;

namespace Banchou {
    public class NudgeToTarget : FSMBehaviour {
        [SerializeField]private float _speed = 8f;
        [SerializeField]private float _duration = 1f;
        [SerializeField]private AnimationCurve _curve = new AnimationCurve();
        [SerializeField]private bool _rotateToTarget = true;
        [SerializeField]private float _rotationSpeed = 1000f;
        [SerializeField]private float _targettingPrecision = 0.8f;
        [Header("Animation Parameters")]
        [SerializeField]private string _onFinish = string.Empty;
        private Transform _orientation;
        private Part.IMovementInput _movement;
        private Part.IMotor _motor;
        private Part.LockOn _lockOn;
        private Rigidbody _body;
        private Transform _target;
        private float _time = 0f;

        public override void Inject(Animator stateMachine) {
            _movement    = stateMachine.GetComponentInChildren<Part.IMovementInput>();
            _motor       = stateMachine.GetComponentInChildren<Part.IMotor>();
            _orientation = stateMachine.GetComponentInChildren<Part.Orientation>().transform;
            _body        = stateMachine.GetComponentInChildren<Rigidbody>();
            _lockOn      = stateMachine.GetComponentInChildren<Part.LockOn>();
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _time = 0f;
            _target = _lockOn.Targets
                .Where(t => {
                    var projected = Vector3.Dot(t.transform.position - _body.transform.position, _orientation.forward);
                    return projected > _targettingPrecision;
                })
                .OrderBy(t => (t.transform.position - _body.transform.position).sqrMagnitude)
                .FirstOrDefault();
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            var dt = Time.deltaTime;
            
            var direction = _orientation.forward;
            if (_target != null) {
                direction = Vector3.ProjectOnPlane(_target.position - _body.transform.position, _body.transform.up).normalized;
            }
            var offset = _curve.Evaluate(_time / _duration) * _speed * dt * direction;
            _motor.Move(offset);

            if (_rotateToTarget) {
                _orientation.rotation = Quaternion.RotateTowards(
                    _orientation.rotation,
                    Quaternion.LookRotation(direction),
                    _rotationSpeed * Time.deltaTime
                );
            }
            
            _time += dt;
        }
    }
}
