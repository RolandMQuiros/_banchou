using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Banchou.State;
using Banchou.Pawn.State;

namespace Banchou.Pawn {
    public static class PawnSelectors {
        public static PawnState GetPawn(this GameState state, string id) {
            var pawns = state.Battle?.Pawns;
            PawnState pawnState;
            if (pawns.TryGetValue(id, out pawnState)) {
                return pawnState;
            }
            return null;
        }
    }
}
