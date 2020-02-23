using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;

namespace Banchou {
    public class DebugDispatcher : MonoBehaviour {
        [Inject] private Dispatcher _dispatch = null;

        [ShowInInspector, DisableInEditorMode, TypeFilter("ActionTypes")]
        private object _action = null;

        private IEnumerable<Type> _actionTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(t => t.Namespace != null)
                .Where(t => t.Namespace.EndsWith("StateAction"))
                .ToList();

        private IEnumerable<Type> ActionTypes() => _actionTypes;
        [Button, DisableInEditorMode]
        [DisableIf("@this._action == null", InfoMessageType.Info)]
        private void Dispatch() {
            _dispatch.Invoke(_action);
        }
    }
}