using UnityEngine;

namespace Banchou.Pawn.FSM {
    public class Attack : FSMBehaviour {
        [SerializeField] private string _button = string.Empty;
        [Header("Animation Parameters")]
        [SerializeField] private string _onAttack = string.Empty;
        [SerializeField] private string _stageOut = string.Empty;

        private int _attackHash;
        private int _stageHash;

        private void OnEnable() {
            _attackHash = Animator.StringToHash(_onAttack);
            _stageHash = Animator.StringToHash(_stageOut);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (Input.GetButtonUp(_button)) {
                animator.SetTrigger(_attackHash);
                animator.SetInteger(_stageHash, animator.GetInteger(_stageHash));
            }
        }
    }
}