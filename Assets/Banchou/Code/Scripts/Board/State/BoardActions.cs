using System;
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
        private Transform _pawnParent;
        private PawnCatalog _pawnCatalog;
        private CreatePawnInstance _createPawn;
        private DestroyPawnInstance _destroyPawn;

        public BoardActions(
            Transform pawnParent,
            PawnCatalog pawnCatalog,
            CreatePawnInstance createPawn,
            DestroyPawnInstance destroyPawn
        ) {
            _pawnParent = pawnParent;
            _pawnCatalog = pawnCatalog;
            _createPawn = createPawn;
            _destroyPawn = destroyPawn;
        }

        public ActionsCreator<GameState> AddPawn(
            string prefabKey,
            string displayName = null,
            float cameraWeight = 0f,
            Vector3 position = default(Vector3)
        ) => (dispatch, getState) => {
            var id = Guid.NewGuid().ToString();
            var instance = _createPawn.Invoke(id, prefabKey, position);
            instance.name = string.IsNullOrWhiteSpace(displayName) ? 
                $"[{prefabKey}] {id}" :
                displayName;

            dispatch(new StateAction.AddPawn {
                ID = id,
                PrefabKey = prefabKey,
                DisplayName = displayName,
                CameraWeight = cameraWeight,
                Position = position
            });
        };

        public ActionsCreator<GameState> RemovePawn(string id) =>
            (dispatch, getState) => {
                // Dispatch before actually destroying, so subscribers have a change to clean up
                dispatch(new StateAction.RemovePawn { ID = id });
                _destroyPawn(id);
            };
    }
}
