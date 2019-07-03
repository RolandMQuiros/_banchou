using UnityEngine;

namespace Banchou {
    public class FiniteStateMachine : MonoBehaviour {
        [SerializeField] private Animator _source = null;
        private void Start() {
            foreach (var behaviour in _source.GetBehaviours<FSMBehaviour>()) {
                behaviour.Inject(_source);
            }
        }

        private void OnAnimatorMove() { }
    }

}