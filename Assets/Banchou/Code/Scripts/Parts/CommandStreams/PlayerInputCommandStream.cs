using System;
using UnityEngine;
using Zenject;

using Banchou.Combatant;

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