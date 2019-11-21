using System;
using System.Linq;
using UnityEngine;

using Zenject;
using Sirenix.OdinInspector;
using Redux;
using Redux.Reactive;

using Banchou.State;

namespace Banchou {
    public delegate GameState GetState();
    public delegate object Dispatcher(object action);
    
    public class StoreInstaller : MonoInstaller {
        [SerializeField, AssetsOnly] private GameState _defaultState = null;
        public override void InstallBindings() {
            var middleware = GetComponents<IMiddleware<GameState>>();
            var store = new Store<GameState>(
                GameStateReducer.Reduce,
                _defaultState,
                middleware
                    .Select(m => m.Middleware)
                    .ToArray()
            );

            Container.BindInstance<GetState>(store.GetState);
            Container.BindInstance<Dispatcher>(store.Dispatch);
            Container.BindInstance<IObservable<GameState>>(store.ObserveState());
        }
    }
}