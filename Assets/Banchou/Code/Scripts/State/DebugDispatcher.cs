using System;
using UnityEngine;

namespace Banchou {
    public class DebugDispatcher : MonoBehaviour, IStoreConnector {
        [SerializeField] private string _actionBody;

        public void Inject(IObservable<State> observeState, Func<object, object> dispatch) {
            
        }
    }
}