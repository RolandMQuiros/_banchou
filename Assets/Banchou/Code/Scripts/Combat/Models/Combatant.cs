using UnityEngine;

namespace Banchou.Combat {
    public class Combatant {
        public readonly Vector2Int Position;
        public Combatant(
            Combatant prev = null,
            Vector2Int? position = null
        ) {
            Position = prev?.Position ?? position ?? Vector2Int.zero;
        }
    }
}