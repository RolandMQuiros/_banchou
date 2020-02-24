using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnState {
        public interface IQueuedCommand { }
        public string ID;
        public string PrefabKey = string.Empty;
        public string DisplayName = string.Empty;
        public int InstanceID = -1;
        public int Team = 0;
        public int Health = 0;
        public float CameraWeight = 0f;
        public Vector3 InitialPosition = Vector3.zero;
        public Vector3 Push = Vector3.zero;
        public IEnumerable<IQueuedCommand> Commands = Enumerable.Empty<IQueuedCommand>();
        public DateTime Created = DateTime.UtcNow;
        public DateTime Updated = DateTime.UtcNow;
        public PawnState() { }
        public PawnState(in PawnState prev) {
            ID = prev?.ID ?? string.Empty;
            PrefabKey = prev?.PrefabKey ?? string.Empty;
            DisplayName = prev?.DisplayName ?? string.Empty;
            InstanceID = prev?.InstanceID ?? -1;
            Team = prev?.Team ?? 0;
            Health = prev?.Health ?? 0;
            CameraWeight = prev?.CameraWeight ?? 0f;
            InitialPosition = prev?.InitialPosition ?? Vector3.zero;
            Push = prev?.Push ?? Vector3.zero;
            Commands = prev?.Commands ?? Enumerable.Empty<IQueuedCommand>();
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.Now;
        }
    }
}