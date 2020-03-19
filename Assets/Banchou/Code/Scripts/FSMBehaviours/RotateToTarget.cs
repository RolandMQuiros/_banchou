using System.Linq;
using UnityEngine;
using Zenject;

namespace Banchou.FSM {
    public class RotateToTarget : FSMBehaviour {
        [SerializeField] private float _targettingPrecision = 0.4f;
        [SerializeField]private float _rotationSpeed = 1000f;

        [Inject] private Part.Orientation _orientation = null;
        [Inject] private Part.LockOn _lockOn = null;
        [Inject] private Part.IMovementInput _movement = null;
        [Inject] private Rigidbody _body = null;
        private Transform _target;

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            _target = _lockOn.Targets
                .Where(t => {
                    var projected = Vector3.Dot(
                        (t.transform.position - stateMachine.transform.position).normalized,
                        _orientation.transform.forward
                    );
                    return projected > _targettingPrecision;
                })
                .OrderBy(t => (t.transform.position - _body.transform.position).sqrMagnitude)
                .FirstOrDefault();
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            var controlAxes = _movement.MovementDirection;
            if (_target != null) {
                var direction = Vector3.ProjectOnPlane(
                    _target.position - _body.transform.position,
                    _body.transform.up
                ).normalized;
                _orientation.transform.rotation = Quaternion.RotateTowards(
                    _orientation.transform.rotation,
                    Quaternion.LookRotation(direction),
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}