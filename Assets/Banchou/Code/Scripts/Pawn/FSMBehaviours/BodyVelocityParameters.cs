using UnityEngine;
using Zenject;

namespace Banchou.FSM {
    public class BodyVelocityParameters : StateMachineBehaviour {
        [Header("State Parameters")]
        [SerializeField] private string _xSpeed = string.Empty;
        [SerializeField] private string _ySpeed = string.Empty;
        [SerializeField] private string _zSpeed = string.Empty;
        [Inject] private Rigidbody _body = null;
        private int _xSpeedHash, _ySpeedHash, _zSpeedHash;

        private void OnEnable() {
            _xSpeedHash = Animator.StringToHash(_xSpeed);
            _ySpeedHash = Animator.StringToHash(_ySpeed);
            _zSpeedHash = Animator.StringToHash(_zSpeed);
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!string.IsNullOrWhiteSpace(_xSpeed)) {
                stateMachine.SetFloat(_xSpeedHash, _body.velocity.x);
            }

            if (!string.IsNullOrWhiteSpace(_ySpeed)) {
                stateMachine.SetFloat(_ySpeedHash, _body.velocity.y);
            }

            if (!string.IsNullOrWhiteSpace(_zSpeed)) {
                stateMachine.SetFloat(_zSpeedHash, _body.velocity.z);
            }
        }
    }
}