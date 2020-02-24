﻿using System;
using UnityEngine;
using Redux;
using Banchou.Pawn;

namespace Banchou.Board {
    namespace StateAction {
        public class AddPawn {
            public string ID;
            public string PrefabKey;
            public string DisplayName;
            public float CameraWeight;
            public Vector3 Position;
        }

        public class RemovePawn {
            public string ID;
        }
    }

    public class BoardActions {
        public StateAction.AddPawn AddPawn(
            string prefabKey,
            string displayName = null,
            float cameraWeight = 0f,
            Vector3 position = default(Vector3)
        ) => new StateAction.AddPawn {
            PrefabKey = prefabKey,
            DisplayName = displayName,
            CameraWeight = cameraWeight,
            Position = position
        };

        public StateAction.RemovePawn RemovePawn(string id) =>
            new StateAction.RemovePawn { ID = id };
    }
}
