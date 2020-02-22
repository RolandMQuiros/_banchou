using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

namespace Banchou.Pawn {
    [CreateAssetMenu(fileName = "Pawn Catalog", menuName = "Banchou/Pawn Catalog")]
    public class PawnCatalog : SerializedScriptableObject {
        public struct CatalogTuple {
            [Required] public string Key;
            [AssetsOnly] public GameObject Value;
        }
        
        [TableMatrix(
            SquareCells = true,
            DrawElementMethod = "DrawCell",
            HideColumnIndices = true,
            HideRowIndices = true,
            ResizableColumns = false,
            IsReadOnly = false
        )]
        [SerializeField]
        private CatalogTuple[,] _catalog = new CatalogTuple[8, 8];
        public Dictionary<string, GameObject> Prefabs =>
            _catalog.Cast<CatalogTuple>()
                .ToDictionary(p => p.Key, p => p.Value);
        
        #if UNITY_EDITOR
            private static CatalogTuple DrawCell(Rect rect, CatalogTuple tuple) {
                tuple.Value = (GameObject) SirenixEditorFields.UnityPreviewObjectField(
                    new Rect(rect) { x = rect.x - 8f,  yMax = rect.yMax - 16f },
                    tuple.Value,
                    typeof(GameObject),
                    dragOnly: false,
                    allowMove: true,
                    allowSwap: true,
                    allowSceneObjects: false
                );

                tuple.Key = SirenixEditorFields.TextField(
                    new Rect(rect) { yMin = rect.yMax - 16f },
                    tuple.Key
                );

                return tuple;
            }
        #endif
    }
}