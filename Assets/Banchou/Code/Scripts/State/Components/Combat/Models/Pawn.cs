using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou {
    public class Pawn {
        public interface IQueuedCommand { }
        public string ID;
        public int Team;
        public int Health;
        public Vector3 Push;
        public IEnumerable<IQueuedCommand> Commands;
        public Pawn(
            in Pawn prev = null,
            string id = null,
            int? team = null,
            int? health = null,
            Vector3? push = null,
            IEnumerable<IQueuedCommand> commands = null
        ) {
            ID = id ?? prev?.ID ?? ID;
            Team = team ?? prev?.Team ?? 0;
            Health = health ?? prev?.Health ?? 0;
            Push = push ?? prev?.Push ?? Vector3.zero;
            Commands = commands ?? prev?.Commands ?? Enumerable.Empty<IQueuedCommand>();
        }
    }
}