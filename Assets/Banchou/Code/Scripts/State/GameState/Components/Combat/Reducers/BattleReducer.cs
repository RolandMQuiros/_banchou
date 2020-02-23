﻿using System.Collections.Generic;

using Banchou.Pawn;

namespace Banchou.Combat {
    internal static class BattleReducer {
        public static BattleState Reduce(in BattleState prev, in object action) {
            var pawnAction = action as Pawn.StateAction.PawnAction;
            if (pawnAction != null) {
                PawnState pawn;
                if (prev.Pawns.TryGetValue(pawnAction.PawnID, out pawn)) {
                    // Check for team damage
                    var damage = action as Pawn.StateAction.DamagePawn;
                    if (damage != null) {
                        var other = prev.Pawns.Get(damage?.From);
                        // TODO?: Friendly fire
                        if (other != null && other.Team == pawn.Team ) {
                            return prev;
                        }
                    }
                    return new BattleState(
                        prev,
                        pawns: new Dictionary<string, PawnState>(prev.Pawns) {
                            [pawnAction.PawnID] = PawnReducer.Reduce(pawn, action)
                        }
                    );
                }
            }
            return prev;
        }
    }
}