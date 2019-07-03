using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou {
    public class CombatGrid : MonoBehaviour {
        [SerializeField] private Vector2 _worldSize = Vector2.zero;
        [SerializeField] private Vector2Int _gridSize = Vector2Int.zero;

        public Vector3 CellPoint(Vector2Int coordinates) {
            return transform.position + new Vector3(
                coordinates.x * _worldSize.x / _gridSize.x,
                0f,
                coordinates.y * _worldSize.y / _gridSize.y
            );
        }

        private void OnDrawGizmos() {
            var cellSize = new Vector2(
                _worldSize.x / _gridSize.x,
                _worldSize.y / _gridSize.y
            );

            for (int x = 0; x <= _gridSize.x; x++) {
                var start = transform.position + Vector3.right * x * cellSize.x;
                var end = start + Vector3.forward * cellSize.y * _gridSize.y;
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= _gridSize.y; y++) {
                var start = transform.position + Vector3.forward * y * cellSize.y;
                var end = start + Vector3.right * cellSize.x * _gridSize.x;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}