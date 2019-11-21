using Banchou.State;
using Banchou.Pawn.State;

namespace Banchou.Combat.State {
    public static class CombatSelectors {
        public static PawnState GetPawn(this GameState state, string pawnID) {
            PawnState pawn;
            if (state.Battle.Pawns.TryGetValue(pawnID, out pawn)) {
                return pawn;
            }
            return null;
        }
    }
}