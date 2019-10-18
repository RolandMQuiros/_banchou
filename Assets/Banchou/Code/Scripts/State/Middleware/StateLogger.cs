using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Redux;
using Banchou;

namespace Banchou.Middleware {
    public class StateLogger : MonoBehaviour, IMiddleware<State> {
        [Serializable]
        public class Log {
            public object Action;
            public State State;
        }

        public List<Log> History { get; private set; } = new List<Log>();

        public Middleware<State> Run => (IStore<State> store) => (Dispatcher next) => (object action) => {
            var result = next(action);
            if (isActiveAndEnabled) {
                var nextState = store.GetState();
                History.Add(new Log {
                    Action = action,
                    State = nextState
                });
            }
            return action;
        };
    }
}
