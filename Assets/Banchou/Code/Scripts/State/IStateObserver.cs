using System;
using UnityEngine;
using Banchou.State;

namespace Banchou {
    public interface IStoreConnector {
        void Inject(IObservable<GameState> observeState, Func<object, object> dispatch);
    }

    public class DispatcherBehaviour : MonoBehaviour, IStoreConnector {
        protected Func<object, object> Dispatcher { get; private set; }

        public void Inject(IObservable<GameState> observeState, Func<object, object> dispatch) {
            Dispatcher = dispatch;
        }
    }
}
