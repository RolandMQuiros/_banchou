using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Redux;
using UniRx;
using Sirenix.OdinInspector;
using Zenject;

using Banchou.State;

namespace Banchou.Middleware {
    public class StateLogger : MonoBehaviour, IMiddleware<GameState> {
        private class Log {
            [ReadOnly, TableColumnWidth(30)] public object Action;
            [ReadOnly, TableColumnWidth(55)] public GameState State;
            private Action _jump;
            public Log(Action jump) { _jump = jump; }
            [Button("Jump"), TableColumnWidth(15)] public void Jump() { _jump(); }
        }
        [ShowInInspector, ReadOnly, HideLabel] private GameState _currentState = null;
        [ShowInInspector, TableList, ListDrawerSettings(IsReadOnly = true)] private List<Log> _history = new List<Log>();
        private int _activeLog;

        public Middleware<GameState> Middleware =>
            store => next => action => {
                var result = next(action);
                if (isActiveAndEnabled) {
                    var nextState = store.GetState();
                    _activeLog++;
                    _history = _history.Take(_activeLog).ToList();

                    var logIndex = _activeLog;
                    _history.Add(new Log(
                        () => {
                            next(new State.Action.HydrateGameState {
                            GameState = nextState
                            });
                            _activeLog = logIndex;
                        }
                    ) {
                        Action = action,
                        State = nextState,
                    });
                }
                return action;
            };

        [Inject]
        public void Connect(IObservable<GameState> observeState, Func<object, object> dispatch) {
            observeState.Subscribe(state => { _currentState = state; });
        }
    }
}
