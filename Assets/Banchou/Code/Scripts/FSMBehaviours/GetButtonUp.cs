using UnityEngine;

namespace Banchou {
    public class GetButtonUp : FSMBehaviour {
        [SerializeField] private string _button = string.Empty;
        [SerializeField] private string _event = string.Empty;
        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (Input.GetButtonUp(_button)) {
                stateMachine.SetTrigger(_event);
            }
        }
    }
}