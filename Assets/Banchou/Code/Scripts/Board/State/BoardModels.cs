using System;
using System.Collections.Generic;
using Banchou.Pawn;

using Banchou.Combatant;

namespace Banchou.Board {
    public class BoardState {
        public Dictionary<Guid, PawnState> Pawns = new Dictionary<Guid, PawnState>();
        public Dictionary<Guid, CombatantState> Combatants = new Dictionary<Guid, CombatantState>();

        public BoardState() { }
        public BoardState(BoardState prev) {
            Pawns = prev?.Pawns ?? new Dictionary<Guid, PawnState>();
            Combatants = prev?.Combatants ?? new Dictionary<Guid, CombatantState>();
        }
    }
}