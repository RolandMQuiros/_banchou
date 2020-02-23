using Banchou.Pawn;

namespace Banchou.Board {
    public static class BoardSelectors {
        public static PawnState GetPawn(this GameState state, string pawnID) {
            PawnState pawn;
            if (state.Board.Pawns.TryGetValue(pawnID, out pawn)) {
                return pawn;
            }
            return null;
        }
    }
}