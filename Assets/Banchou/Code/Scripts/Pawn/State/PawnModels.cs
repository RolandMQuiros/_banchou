using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnState {
        public interface IQueuedCommand { }
        public string ID;
        public string PrefabKey;
        public int Team;
        public int Health;
        public Vector3 Push;
        public IEnumerable<IQueuedCommand> Commands = Enumerable.Empty<IQueuedCommand>();
        public PawnState(
            in PawnState prev = null,
            string id = null,
            string prefabKey = null,
            int? team = null,
            int? health = null,
            Vector3? push = null,
            IEnumerable<IQueuedCommand> commands = null
        ) {
            ID = id ?? prev?.ID ?? ID;
            PrefabKey = prefabKey ?? prev?.PrefabKey ?? PrefabKey;
            Team = team ?? prev?.Team ?? 0;
            Health = health ?? prev?.Health ?? 0;
            Push = push ?? prev?.Push ?? Vector3.zero;
            Commands = commands ?? prev?.Commands ?? Enumerable.Empty<IQueuedCommand>();
        }
    }
}