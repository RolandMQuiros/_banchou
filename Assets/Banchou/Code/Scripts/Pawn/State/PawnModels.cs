using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnState {
        public interface IQueuedCommand { }
        public string ID;
        public string PrefabKey;
        public string DisplayName;
        public int Team;
        public int Health;
        public Vector3 InitialPosition;
        public Vector3 Push;
        public IEnumerable<IQueuedCommand> Commands = Enumerable.Empty<IQueuedCommand>();
        public DateTime Created;
        public DateTime Updated;
        public PawnState() { }
        public PawnState(in PawnState prev) {
            ID = prev?.ID ?? string.Empty;
            PrefabKey = prev?.PrefabKey ?? string.Empty;
            DisplayName = prev?.DisplayName ?? string.Empty;
            Team = prev?.Team ?? 0;
            Health = prev?.Health ?? 0;
            InitialPosition = prev?.InitialPosition ?? Vector3.zero;
            Push = prev?.Push ?? Vector3.zero;
            Commands = prev?.Commands ?? Enumerable.Empty<IQueuedCommand>();
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.Now;
        }
    }
}