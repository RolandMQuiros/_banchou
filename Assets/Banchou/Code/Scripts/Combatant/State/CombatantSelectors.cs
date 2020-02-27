using System;

namespace Banchou.Combatant {
    public static class CombatantSelectors {
        public static CombatantState GetCombatant(this GameState state, Guid id) {
            return state.Board.Combatants[id];
        }
    }
}