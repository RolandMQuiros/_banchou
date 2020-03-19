using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.FSM {
    public class FSMBehaviour : StateMachineBehaviour {
        private Dictionary<int, List<IDisposable>> _streams = new Dictionary<int, List<IDisposable>>();

        protected void AddStreams(AnimatorStateInfo stateInfo, IEnumerable<IDisposable> streams) {
            List<IDisposable> list;
            var stateHash = stateInfo.fullPathHash;
            if (!_streams.TryGetValue(stateHash, out list)) {
                list = new List<IDisposable>();
                _streams[stateHash] = list;
            }
            foreach (var stream in streams) { list.Add(stream); }
		}

        protected void AddStreams(AnimatorStateInfo stateInfo, params IDisposable[] streams) {
			List<IDisposable> list;
            var stateHash = stateInfo.fullPathHash;
            if (!_streams.TryGetValue(stateHash, out list)) {
                list = new List<IDisposable>();
                _streams[stateHash] = list;
            }
            foreach (var stream in streams) { list.Add(stream); }
		}
		
        public override void OnStateExit(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            List<IDisposable> toDispose;
            if (_streams.TryGetValue(stateInfo.fullPathHash, out toDispose)) {
                toDispose.ForEach(s => s.Dispose());
            }
        }
    }
}