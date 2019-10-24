using System;
using UnityEngine;

namespace Banchou {
    public interface IStoreConnector {
        void Inject(IObservable<State> observeState, Func<object, object> dispatch);
    }

    public class DispatcherBehaviour : MonoBehaviour, IStoreConnector {
        protected Func<object, object> Dispatch { get; private set; }

        public void Inject(IObservable<State> observeState, Func<object, object> dispatch) {
            Dispatch = dispatch;
        }
    }
}
