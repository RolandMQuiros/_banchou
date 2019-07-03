using System.Linq;
using UnityEngine;

namespace Banchou {
    public class RotateToTarget : FSMBehaviour {
        [SerializeField] private float _targettingPrecision = 0.4f;
        [SerializeField]private float _rotationSpeed = 1000f;

        private Transform _orientation;
        private Part.LockOn _lockOn;
        private Part.IMovementInput _movement;
        private Rigidbody _body;
        private Transform _target;

        public override void Inject(Animator stateMachine) {
            _orientation   = stateMachine.GetComponentInChildren<Part.Orientation>().transform;
            _lockOn        = stateMachine.GetComponentInChildren<Part.LockOn>();
            _movement      = stateMachine.GetComponentInChildren<Part.IMovementInput>();
            _body          = stateMachine.GetComponentInChildren<Rigidbody>();
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _target = _lockOn.Targets
                .Where(t => {
                    var projected = Vector3.Dot((t.transform.position - stateMachine.transform.position).normalized, _orientation.forward);
                    return projected > _targettingPrecision;
                })
                .OrderBy(t => (t.transform.position - _body.transform.position).sqrMagnitude)
                .FirstOrDefault();
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            var controlAxes = _movement.MovementDirection;
            if (_target != null) {
                var direction = Vector3.ProjectOnPlane(_target.position - _body.transform.position, _body.transform.up).normalized;
                _orientation.rotation = Quaternion.RotateTowards(
                    _orientation.rotation,
                    Quaternion.LookRotation(direction),
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}