using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;
using Redux;
using Redux.Reactive;

using Banchou.State;

namespace Banchou {
    public class StateContext : MonoBehaviour {
        [SerializeField] private TextAsset _initialState = null;

        private Store<GameState> _store;
        private HashSet<IStoreConnector> _connectors;

        public object Dispatch(object action) => _store.Dispatch(action);

        private void Awake() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                Converters = new List<JsonConverter> {
                    new Vector3JSONConverter()
                }
            };
        }

        private void Start() {
            var middleware = GetComponents<IMiddleware<GameState>>()
                .Select(m => m.Middleware)
                .ToArray();
            
            _store = new Store<GameState>(
                Reducers.GameStateReducer,
                _initialState != null ? JsonConvert.DeserializeObject<GameState>(_initialState.text) : new GameState(),
                middleware
            );

            _connectors = new HashSet<IStoreConnector>(
                transform.GetComponentsInChildren<IStoreConnector>(true)
            );

            foreach (var injectable in _connectors) {
                injectable.Inject(_store.ObserveState(), _store.Dispatch);
            }
        }

        private void OnTransformChildrenChanged() {
            var newConnectors = transform.GetComponentsInChildren<IStoreConnector>(true)
                .Except(_connectors);

            foreach (var connector in newConnectors) {
                connector.Inject(_store.ObserveState(), _store.Dispatch);
                _connectors.Add(connector);
            }
        }
    }
}