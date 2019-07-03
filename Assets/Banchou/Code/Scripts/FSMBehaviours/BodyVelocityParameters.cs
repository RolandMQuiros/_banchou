using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.FSM {
    public class BodyVelocityParameters : FSMBehaviour {
        [Header("State Parameters")]
        [SerializeField] private string _xSpeed = string.Empty;
        [SerializeField] private string _ySpeed = string.Empty;
        [SerializeField] private string _zSpeed = string.Empty;
        private Rigidbody _body;
        private int _xSpeedHash, _ySpeedHash, _zSpeedHash;

        public override void Inject(Animator stateMachine) {
            _body = stateMachine.GetComponentInChildren<Rigidbody>();
            _xSpeedHash = Animator.StringToHash(_xSpeed);
            _ySpeedHash = Animator.StringToHash(_ySpeed);
            _zSpeedHash = Animator.StringToHash(_zSpeed);
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (!string.IsNullOrEmpty(_xSpeed)) {
                stateMachine.SetFloat(_xSpeedHash, _body.velocity.x);
            }

            if (!string.IsNullOrEmpty(_ySpeed)) {
                stateMachine.SetFloat(_ySpeedHash, _body.velocity.y);
            }

            if (!string.IsNullOrEmpty(_ySpeed)) {
                stateMachine.SetFloat(_ySpeedHash, _body.velocity.y);
            }
        }
    }
}