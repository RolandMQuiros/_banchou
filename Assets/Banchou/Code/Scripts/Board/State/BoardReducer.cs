using System;
using System.Collections.Generic;

using Banchou.Pawn;

namespace Banchou.Board {
    internal static class BoardReducer {
        public static BoardState Reduce(in BoardState prev, in object action) {
            var addPawn = action as StateAction.AddPawn;
            if (addPawn != null) {
                var pawnID = Guid.NewGuid().ToString();
                return new BoardState(prev) {
                    Pawns = new Dictionary<string, PawnState>(prev.Pawns) {
                        [pawnID] = new PawnState {
                            ID = pawnID,
                            PrefabKey = addPawn.PrefabKey,
                            DisplayName = string.IsNullOrWhiteSpace(addPawn.DisplayName) ?
                                $"[{addPawn.PrefabKey}] {pawnID}" :
                                addPawn.DisplayName,
                            Health = 100,
                            InitialPosition = addPawn.Position   
                        }
                    }
                };
            }

            var removePawn = action as StateAction.RemovePawn;
            if (removePawn != null && prev.Pawns.ContainsKey(removePawn.ID)) {
                var newPawns = new Dictionary<string, PawnState>(prev.Pawns);
                newPawns.Remove(removePawn.ID);
                return new BoardState(prev) {
                    Pawns = newPawns
                };
            }

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
                    return new BoardState(prev) {
                        Pawns = new Dictionary<string, PawnState>(prev.Pawns) {
                            [pawnAction.PawnID] = PawnReducer.Reduce(pawn, action)
                        }
                    };
                }
            }
            return prev;
        }
    }
}