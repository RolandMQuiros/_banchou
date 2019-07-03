﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UniRx;

namespace Banchou.FSM {
    public class CommandEvents : FSMBehaviour {
        [SerializeField] private Part.Command[] _commands = null;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private bool _filter = false;
        private Part.ICommandStream _commandStream;
        private Dictionary<Part.Command, int> _lookup;

        public override void Inject(Animator stateMachine) {
            _commandStream = stateMachine.GetComponentInChildren<Part.ICommandStream>();
            IEnumerable<Part.Command> commands;
            if (_filter) {
                commands = Enum.GetValues(typeof(Part.Command))
                    .Cast<Part.Command>()
                    .Except(_commands);
            } else {
                commands = _commands;
            }

            _lookup = commands
                .Join(
                    stateMachine.parameters,
                    inner => Regex.Replace($"[Command] {inner.ToString()}", "([A-Z])([A-Z])([a-z])|([a-z])([A-Z])", "$1$4 $2$3$5"),
                    outer => outer.name,
                    (inner, outer) => new KeyValuePair<Part.Command, int>(inner, outer.nameHash)
                )
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            AddStreams(
                stateInfo,
                _commandStream.Commands
                    .Subscribe(command => {
                        int hash;
                        var currentStateInfo = stateMachine.GetCurrentAnimatorStateInfo(layerIndex);
                        if (currentStateInfo.normalizedTime >= _delay && _lookup.TryGetValue(command, out hash)) {
                            stateMachine.SetTrigger(hash);
                        }
                    })
            );
        }
    }
}