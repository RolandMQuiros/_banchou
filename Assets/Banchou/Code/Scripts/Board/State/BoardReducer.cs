using System;
using System.Collections.Generic;

using Banchou.Pawn;
using Banchou.Combatant;

namespace Banchou.Board {
    internal static class BoardReducer {
        public static BoardState Reduce(in BoardState prev, in object action) =>
            ApplyPawnActions(prev, action) ??
            ApplyCombatantActions(prev, action) ??
            prev;

        private static BoardState ApplyPawnActions(in BoardState prev, in object action) {
            var addPawn = action as StateAction.AddPawn;
            if (addPawn != null) {
                var id = Guid.NewGuid().ToString();
                var pawns = new Dictionary<string, PawnState>(prev.Pawns) {
                    [id] = new PawnState {
                        ID = id,
                        PrefabKey = addPawn.PrefabKey,
                        DisplayName = addPawn.DisplayName,
                        CameraWeight = addPawn.CameraWeight,
                        InitialPosition = addPawn.Position
                    }
                };

                var addCombatant = action as StateAction.AddCombatant;
                var combatants = prev.Combatants;
                if (addCombatant != null) {
                    combatants = new Dictionary<string, CombatantState>(prev.Combatants) {
                        [id] = new CombatantState {
                            PawnID = id,
                            Health = addCombatant.Health
                        }
                    };
                }

                return new BoardState(prev) {
                    Pawns = pawns,
                    Combatants = combatants
                };
            }

            var removePawn = action as StateAction.RemovePawn;
            if (removePawn != null && prev.Pawns.ContainsKey(removePawn.ID)) {
                var pawns = new Dictionary<string, PawnState>(prev.Pawns);
                pawns.Remove(removePawn.ID);

                var combatants = prev.Combatants;
                if (prev.Combatants.ContainsKey(removePawn.ID)) {
                    combatants.Remove(removePawn.ID);
                }

                return new BoardState(prev) {
                    Pawns = pawns,
                    Combatants = combatants
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

            return null;
        }

        private static BoardState ApplyCombatantActions(in BoardState prev, in object action) {
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
            return null;
        }
    }
}