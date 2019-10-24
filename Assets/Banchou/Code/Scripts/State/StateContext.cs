using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;
using Redux;
using Redux.Reactive;

namespace Banchou {
    public class StateContext : MonoBehaviour {
        [SerializeField] private TextAsset _initialState = null;

        private Store<State> _store;
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
            var middleware = GetComponents<IMiddleware<State>>()
                .Select(m => m.Run)
                .ToArray();
            
            _store = new Store<State>(
                (in State prev, in object action) => new State(
                    prev,
                    battle: Reducers.BattleReducer(prev.Battle, action)
                ),
                _initialState != null ? JsonConvert.DeserializeObject<State>(_initialState.text) : new State(),
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