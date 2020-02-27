using System;
using System.Collections.Generic;
using UniRx;
using Zenject;
using UnityEngine;

namespace Banchou.Pawn {
    public class PawnInstaller : MonoInstaller {
        public Guid PawnID => _pawnID;
        private Guid _pawnID;
        private IObservable<GameState> _observeState;

        [Inject]
        public void Construct(Guid pawnID) {
            _pawnID = pawnID;
        }

        public override void InstallBindings() {            
            Container.BindInstance(_pawnID);
        }
    }
}