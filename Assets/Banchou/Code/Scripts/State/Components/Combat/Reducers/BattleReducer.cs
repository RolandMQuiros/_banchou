using System.Collections.Generic;

namespace Banchou {
    public static partial class Reducers {
        public static Battle BattleReducer(in Battle prev, in object action) {
            var pawnAction = action as Action.PawnAction;
            if (pawnAction != null) {
                Pawn pawn;
                if (prev.Pawns.TryGetValue(pawnAction.PawnID, out pawn)) {
                    // Check for team damage
                    var damage = action as Action.DamagePawn;
                    if (damage != null) {
                        var other = prev.Pawns.Get(damage?.From);
                        // TODO?: Friendly fire
                        if (other != null && other.Team == pawn.Team ) {
                            return prev;
                        }
                    }
                    return new Battle(
                        prev,
                        pawns: new Dictionary<string, Pawn>(prev.Pawns) {
                            [pawnAction.PawnID] = PawnReducer(pawn, action)
                        }
                    );
                }
            }
            return prev;
        }
    }
}