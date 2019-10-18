using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Banchou.FSM {
    public class MotorRootMotion : FSMBehaviour {
        [SerializeField] private bool _rootPosition = true;
        private Part.IMotor _motor;
        private Transform _orientation;
        public override void Inject(Animator stateMachine) {
            _motor = stateMachine.GetComponentInChildren<Part.IMotor>();
            _orientation = stateMachine.GetComponentInChildren<Part.Orientation>().transform;
        }

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