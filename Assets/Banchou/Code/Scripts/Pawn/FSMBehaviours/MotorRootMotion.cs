using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Banchou.FSM {
    public class MotorRootMotion : FSMBehaviour {
        [SerializeField] private bool _rootPosition = true;
        [Inject] private Part.IMotor _motor = null;

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (_rootPosition) {
                AddStreams(
                    stateInfo,
                    stateMachine.OnAnimatorMoveAsObservable()
                        .Subscribe(_ => {
                            _motor.Move(stateMachine.deltaPosition); 
                        })
                );
            }
        }
    }
}