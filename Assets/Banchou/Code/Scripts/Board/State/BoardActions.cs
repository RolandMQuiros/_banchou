using UnityEngine;

namespace Banchou.Board {
    namespace StateAction {
        public class AddPawn {           
            public string PrefabKey;
            public string DisplayName;
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
            Vector3 position = default(Vector3)
        ) => new StateAction.AddPawn { PrefabKey = prefabKey, DisplayName = displayName, Position = position };

        public StateAction.RemovePawn RemovePawn(string id) =>
            new StateAction.RemovePawn { ID = id };
    }
}
