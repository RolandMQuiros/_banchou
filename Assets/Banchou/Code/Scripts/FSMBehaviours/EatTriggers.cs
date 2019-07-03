using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.FSM {
    public class EatTriggers : FSMBehaviour {
        [Flags]
        private enum When { OnEnter, OnUpdate, OnExit }
        [SerializeField] private When _when = When.OnUpdate;
        [SerializeField] private string[] _triggers = null;
        private HashSet<int> _hashes;

        public override void Inject(Animator stateMachine) {
            _hashes = new HashSet<int>(
                _triggers.Join(
                    stateMachine.parameters,
                    inner => inner,
                    outer => outer.name,
                    (inner, outer) => outer.nameHash
                )
            );
        }

        private void ResetTriggers(Animator stateMachine) {
            foreach (var hash in _hashes) {
                stateMachine.ResetTrigger(hash);
            }
        }

        public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (_when == When.OnEnter) {
                ResetTriggers(stateMachine);
            }
        }

        public override void OnStateExit(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            base.OnStateExit(stateMachine, stateInfo, layerIndex);
            if (_when == When.OnExit) {
                ResetTriggers(stateMachine);
            }
        }

        public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            if (_when == When.OnUpdate) {
                ResetTriggers(stateMachine);
            }
        }
    }
}