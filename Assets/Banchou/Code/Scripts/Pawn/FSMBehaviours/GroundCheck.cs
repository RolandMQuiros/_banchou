using UnityEngine;

namespace Banchou.FSM {
    public class GroundCheck : FSMBehaviour {
        [SerializeField] private string _groundedParameter = string.Empty;
        private Part.GroundedVolume _grounded;
        private int _groundedHash;
        public override void Inject(Animator stateMachine) {
            _grounded = stateMachine.GetComponentInChildren<Part.GroundedVolume>();
            _groundedHash = Animator.StringToHash(_groundedParameter);
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            stateMachine.SetBool(_groundedHash, _grounded.IsGrounded);
        }
    }
}