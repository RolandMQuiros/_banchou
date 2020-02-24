using System;
using System.Collections.Generic;

using Banchou.Pawn;
using Banchou.Combatant;

namespace Banchou.Board {
    internal static class BoardReducer {
        public static BoardState Reduce(in BoardState prev, in object action) {
            var addPawn = action as StateAction.AddPawn;
            if (addPawn != null) {
                var id = Guid.NewGuid().ToString();
                return new BoardState(prev) {
                    Pawns = new Dictionary<string, PawnState>(prev.Pawns) {
                        [id] = new PawnState {
                            ID = id,
                            PrefabKey = addPawn.PrefabKey,
                            DisplayName = addPawn.DisplayName,
                            CameraWeight = addPawn.CameraWeight,
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
                if (prev.Pawns.TryGetValue(pawnAction.ID, out pawn)) {
                    return new BoardState(prev) {
                        Pawns = new Dictionary<string, PawnState>(prev.Pawns) {
                            [pawnAction.ID] = PawnReducer.Reduce(pawn, action)
                        }
                    };
                }
            }

            var combatantAction = action as Combatant.StateAction.CombatantAction;
            if (combatantAction != null) {
                CombatantState combatant;
                if (prev.Combatants.TryGetValue(combatantAction.ID, out combatant)) {
                    return new BoardState(prev) {
                        Combatants = new Dictionary<string, CombatantState> {
                            [combatantAction.ID] = CombatantReducer.Reduce(combatant, action)
                        }
                    };
                }
            }

            return prev;
        }
    }
}