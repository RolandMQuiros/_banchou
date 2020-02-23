using System.Collections;
using System.Collections.Generic;

namespace Banchou.Pawn {
    public static class PawnSelectors {
        public static PawnState GetPawn(this GameState state, string id) {
            var pawns = state.Board?.Pawns;
            PawnState pawnState;
            if (pawns.TryGetValue(id, out pawnState)) {
                return pawnState;
            }
            return null;
        }

        public static IEnumerable<PawnState> GetPawns(this GameState state) {
            return state.Board?.Pawns?.Values;
        }
    }
}
