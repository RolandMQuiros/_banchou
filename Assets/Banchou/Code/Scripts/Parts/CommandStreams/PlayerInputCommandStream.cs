using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using Zenject;

namespace Banchou.Part {
    public class PlayerInputCommandStream : MonoBehaviour, ICommandStream {
        public IObservable<Command> Commands { get; private set; }
        public bool IsBuffered(Command command) {
            throw new NotImplementedException();
        }

        [Inject]
        public void Construct(
            
        ) {
            
        }
    }
}