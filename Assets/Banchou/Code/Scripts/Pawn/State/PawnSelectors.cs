using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;

namespace Banchou.Pawn {
    public static class PawnSelectors {
        public static PawnState GetPawn(this GameState state, string id) {
            var pawns = state.Board?.Pawns;
            PawnState pawnState;
            if (pawns.TryGetValue(id, out pawnState)) {
                return pawnState;
            }
            return null;
        }

        public static IEnumerable<PawnState> GetPawns(this GameState state) {
            return state.Board?.Pawns?.Values;
        }

        public static IObservable<IEnumerable<PawnState>> AddedPawns(
            this IObservable<GameState> observeState
        ) {
            return observeState.Select(s => s.GetPawns())
                .DistinctUntilChanged()
                .Pairwise()
                .Select(pair => pair.Current.Except(pair.Previous));
        }

        public static IObservable<IEnumerable<PawnState>> RemovedPawns(
            this IObservable<GameState> observeState
        ) {
            return observeState.Select(s => s.GetPawns())
                .DistinctUntilChanged()
                .Pairwise()
                .Select(pair => pair.Previous.Except(pair.Current));
        }
    }
}
