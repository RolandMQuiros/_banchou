using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UniRx;
using Zenject;
using Sirenix.OdinInspector;

using Banchou.Combatant;

namespace Banchou.FSM {
    public class CommandEvents : FSMBehaviour {
        [SerializeField, DrawWithUnity] private Command[] _commands = null;
        [SerializeField] private bool _filter = false;
        [Inject] private Part.ICommandStream _commandStream = null;
        private Dictionary<Command, int> _lookup;


        [Inject]
        public void Attach(Animator stateMachine) {
            IEnumerable<Command> commands;
            if (_filter) {
                commands = Enum.GetValues(typeof(Command))
                    .Cast<Command>()
                    .Except(_commands);
            } else {
                commands = _commands;
            }

            _lookup = commands
                .Join(
                    stateMachine.parameters,
                    inner => Regex.Replace($"[Command] {inner.ToString()}", "([A-Z])([A-Z])([a-z])|([a-z])([A-Z])", "$1$4 $2$3$5"),
                    outer => outer.name,
                    (inner, outer) => new KeyValuePair<Command, int>(inner, outer.nameHash)
                )
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            AddStreams(
                stateInfo,
                _commandStream.Commands
                    .Subscribe(command => {
                        int hash;
                        if (_lookup.TryGetValue(command, out hash)) {
                            stateMachine.SetTrigger(hash);
                        }
                    })
            );
        }
    }
}