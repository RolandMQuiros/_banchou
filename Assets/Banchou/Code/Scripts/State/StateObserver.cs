﻿using System;
using UnityEngine;

namespace Banchou {
    public class StateObserver : MonoBehaviour {
        public IObservable<GameState> ObserveState => _stream;
        private IObservable<GameState> _stream;
        public void Inject(IObservable<GameState> stream) {
            _stream = stream;
        }
    }
}