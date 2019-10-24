using System.Collections.Generic;

namespace Banchou {
    public static partial class Reducers {
        public static Battle BattleReducer(in Battle prev, in object action) {
            var pawnAction = action as Action.PawnAction;
            if (pawnAction != null) {
                Pawn pawn;
                if (prev.Pawns.TryGetValue(pawnAction.PawnID, out pawn)) {
                    var newPawns = new Dictionary<string, Pawn>(prev.Pawns);
                    newPawns[pawnAction.PawnID] = PawnReducer(pawn, prev.Pawns, action);
                    return new Battle(
                        prev,
                        pawns: newPawns
                    );
                }
            }
            return prev;
        }
    }
}