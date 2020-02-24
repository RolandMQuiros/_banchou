namespace Banchou.Combatant {
    public static class CombatantSelectors {
        public static CombatantState GetCombatant(this GameState state, string id) {
            return state.Board.Combatants[id];
        }
    }
}