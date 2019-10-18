namespace Banchou {
    public static class CombatSelectors {
        public static Pawn GetPawn(this State state, string pawnID) {
            return state.Battle.Pawns[pawnID];
        }
    }
}