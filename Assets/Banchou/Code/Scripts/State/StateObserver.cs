using System;
using UnityEngine;

namespace Banchou {
    public class StateObserver : MonoBehaviour {
        public IObservable<State> ObserveState => _stream;
        private IObservable<State> _stream;
        public void Inject(IObservable<State> stream) {
            _stream = stream;
        }
    }
}