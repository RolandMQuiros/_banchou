using UnityEngine;

namespace Banchou.FSM {
    public class Jump : FSMBehaviour {
        private enum When { OnEnter, OnExit }
        [SerializeField] private When _when = When.OnEnter;
        [SerializeField] private Vector3 _force = new Vector3();
        [SerializeField] private ForceMode _forceMode = ForceMode.VelocityChange;
        [SerializeField] private bool _relativeToBody = false;
        private Rigidbody _body;

        public override void Inject(Animator stateMachine) {
            _body = stateMachine.GetComponentInChildren<Rigidbody>();    
        }

        private void ApplyForce() {
            if (_relativeToBody) { _body.AddRelativeForce(_force, _forceMode); }
            else { _body.AddForce(_force, _forceMode); }
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (_when == When.OnEnter) {
                ApplyForce();
            }
        }

        public override void OnStateExit(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit(stateMachine, stateInfo, layerIndex);
            if (_when == When.OnExit) {
                ApplyForce();
            }
        }
    }
}