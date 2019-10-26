using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.State.Model {
    public class Battle {
        public Dictionary<string, Pawn> Pawns;

        public Battle(
            Battle prev = null,
            Dictionary<string, Pawn> pawns = null
        ) {
            Pawns = pawns ?? prev?.Pawns ?? new Dictionary<string, Pawn>();
        }
    }
}