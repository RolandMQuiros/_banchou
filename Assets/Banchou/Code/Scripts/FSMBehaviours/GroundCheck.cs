using UnityEngine;
using Zenject;

namespace Banchou.FSM {
    public class GroundCheck : FSMBehaviour {
        [SerializeField] private string _groundedParameter = string.Empty;
        [Inject] private Part.GroundedVolume _grounded = null;
        private int _groundedHash;
        
        private void OnEnable() {
            _groundedHash = Animator.StringToHash(_groundedParameter);
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            stateMachine.SetBool(_groundedHash, _grounded.IsGrounded);
        }
    }
}