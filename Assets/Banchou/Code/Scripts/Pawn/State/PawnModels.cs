using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnState {
        public string ID;
        public string PrefabKey = string.Empty;
        public string DisplayName = string.Empty;
        public int InstanceID = -1;
        public float CameraWeight = 0f;
        public Vector3 InitialPosition = Vector3.zero;
        public DateTime Created = DateTime.UtcNow;
        public DateTime Updated = DateTime.UtcNow;
        public PawnState() { }
        public PawnState(in PawnState prev) {
            ID = prev?.ID ?? string.Empty;
            PrefabKey = prev?.PrefabKey ?? string.Empty;
            DisplayName = prev?.DisplayName ?? string.Empty;
            InstanceID = prev?.InstanceID ?? -1;
            CameraWeight = prev?.CameraWeight ?? 0f;
            InitialPosition = prev?.InitialPosition ?? Vector3.zero;
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.Now;
        }
    }
}