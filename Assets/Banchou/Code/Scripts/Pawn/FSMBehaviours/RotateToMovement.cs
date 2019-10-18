using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Banchou {
    public class RotateToMovement : FSMBehaviour {
        [SerializeField] private float _rotationSpeed = 1000f;
        private Transform _orientation;
        private Rigidbody _body;

        public override void Inject(Animator stateMachine) {
            _orientation = stateMachine.GetComponentInChildren<Part.Orientation>().transform;
            _body = stateMachine.GetComponentInChildren<Rigidbody>();
        }

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