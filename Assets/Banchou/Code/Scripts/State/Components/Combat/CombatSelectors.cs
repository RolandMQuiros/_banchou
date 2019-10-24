namespace Banchou {
    public static class CombatSelectors {
        public static Pawn GetPawn(this State state, string pawnID) {
            Pawn pawn;
            if (state.Battle.Pawns.TryGetValue(pawnID, out pawn)) {
                return pawn;
            }
            return null;
        }
    }
}