using System;
using System.Linq;
using Zenject;
using UniRx;

namespace Banchou.Pawn {
    public class PawnSpawner {
        public void ConnectToStore(
            DiContainer container,
            IObservable<GameState> observeState,
            Dispatcher dispatch
        ) {
            var pawnChanges = observeState
                .Select(state => state.GetPawns())
                .DistinctUntilChanged()
                .Pairwise();
                
            pawnChanges
                .SelectMany(pair => pair.Current.Except(pair.Previous))
                .Subscribe(SpawnPawn);
            
            pawnChanges
                .SelectMany(pair => pair.Previous.Except(pair.Current))
                .Subscribe(DestroyPawn);

            void SpawnPawn(PawnState pawnState) {
                
            }

            void DestroyPawn(PawnState pawnState) {

            }
        }
    }
}