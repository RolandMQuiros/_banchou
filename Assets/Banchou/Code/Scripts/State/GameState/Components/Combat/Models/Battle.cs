﻿using System.Collections.Generic;
using Banchou.Pawn.State;

namespace Banchou.Combat.State {
    public class BattleState {
        public Dictionary<string, PawnState> Pawns = new Dictionary<string, PawnState>();

        public BattleState(
            BattleState prev = null,
            Dictionary<string, PawnState> pawns = null
        ) {
            Pawns = pawns ?? prev?.Pawns ?? new Dictionary<string, PawnState>();
        }
    }
}