using System.Collections.Generic;
using Banchou.Pawn;

using Banchou.Combatant;

namespace Banchou.Board {
    public class BoardState {
        public Dictionary<string, PawnState> Pawns = new Dictionary<string, PawnState>();
        public Dictionary<string, CombatantState> Combatants = new Dictionary<string, CombatantState>();

        public BoardState() { }
        public BoardState(BoardState prev) {
            Pawns = prev?.Pawns ?? new Dictionary<string, PawnState>();
            Combatants = prev?.Combatants ?? new Dictionary<string, CombatantState>();
        }
    }
}