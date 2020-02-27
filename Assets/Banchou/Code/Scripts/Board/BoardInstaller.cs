using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

using Banchou.Pawn;
using Banchou.Combatant;

namespace Banchou.Board {
    public class BoardInstaller : MonoInstaller {
        [SerializeField] private Transform _pawnParent = null;
        [SerializeField] private PawnCatalog _catalog = null;

        public override void InstallBindings() {
            Container.BindInstance(_pawnParent).WhenInjectedInto<PawnInstanceFactory>();
            Container.BindInstance(_catalog.Prefabs).WhenInjectedInto<PawnInstanceFactory>();
            Container.BindFactory<PawnState, PawnInstance, PawnInstance.Factory>()
                .FromFactory<PawnInstanceFactory>();
            Container.BindInterfacesTo<PawnInstanceBuilder>()
                .AsSingle()
                .NonLazy();
            Container.Bind<BoardActions>().AsTransient();
            Container.Bind<PawnActions>().AsTransient();
            Container.Bind<CombatantActions>().AsTransient();
        }
    }
}