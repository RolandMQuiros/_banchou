using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Banchou.FSM {
    public class RotateToMovement : FSMBehaviour {
        [SerializeField] private float _rotationSpeed = 1000f;
        [Inject] private Transform _orientation = null;
        [Inject] private Rigidbody _body = null;

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            Vector3 direction = new Vector3();
            AddStreams(
                stateInfo,
                stateMachine.FixedUpdateAsObservable()
                    .Select(_ => stateMachine.transform.position)
                    .DistinctUntilChanged()
                    .Pairwise()
                    .Subscribe(pair => {
                        var diff = pair.Current - pair.Previous;
                        if (diff != Vector3.zero) {
                            direction = Vector3.ProjectOnPlane(diff, _body.transform.up).normalized;
                            _orientation.rotation = Quaternion.RotateTowards(
                                _orientation.rotation,
                                Quaternion.LookRotation(direction),
                                _rotationSpeed * Time.deltaTime
                            );
                        }
                    })
            );
        }
    }
}