using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Banchou {
    public class DebugDispatcher : DispatcherBehaviour {
        [ShowInInspector, DisableInEditorMode, TypeFilter("ActionTypes")]
        private object _action = null;

        private IEnumerable<Type> ActionTypes() =>
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(t => t.Namespace == "Banchou.State.Action");

        [Button, DisableInEditorMode]
        [DisableIf("@this._action == null", InfoMessageType.Info)]
        private void Dispatch() {
            Dispatcher?.Invoke(_action);
        }
    }
}