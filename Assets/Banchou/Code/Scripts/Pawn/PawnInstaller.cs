using System;
using UnityEngine;
using UniRx;
using Zenject;

using Banchou.State;
using Banchou.Pawn.State;

namespace Banchou.Pawn {
    public class PawnInstaller : MonoInstaller {
        private IObservable<GameState> _observeState;
        public string PawnID { get; private set; }

        [Inject]
        public void Construct(
            string pawnID,
            IObservable<GameState> observeState
        ) {
            PawnID = pawnID;
            _observeState = observeState;
        }

        public override void InstallBindings() {
            Container.Bind<string>()
                .WithId("PawnID")
                .FromMethod(() => PawnID);
            
            Container.Bind<IObservable<PawnState>>()
                .FromMethod(
                    () => _observeState.Select(state => state.GetPawn(PawnID))
                        .Where(pawn => pawn != null)
                        .DistinctUntilChanged()
                );
        }
    }
}