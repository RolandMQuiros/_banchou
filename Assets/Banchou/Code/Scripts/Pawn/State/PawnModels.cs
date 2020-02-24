using System;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnState {
        public string ID;
        public string PrefabKey = string.Empty;
        public string DisplayName = string.Empty;
        public float CameraWeight = 0f;
        public Vector3 InitialPosition = Vector3.zero;
        public DateTime Created = DateTime.UtcNow;
        public DateTime Updated = DateTime.UtcNow;
        public PawnState() { }
        public PawnState(in PawnState prev) {
            ID = prev?.ID ?? string.Empty;
            PrefabKey = prev?.PrefabKey ?? string.Empty;
            DisplayName = prev?.DisplayName ?? string.Empty;
            CameraWeight = prev?.CameraWeight ?? 0f;
            InitialPosition = prev?.InitialPosition ?? Vector3.zero;
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.Now;
        }
    }
}