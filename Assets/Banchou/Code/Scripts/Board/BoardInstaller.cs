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
            Container.BindInterfacesTo<PawnInstanceBuilder>()
                .FromMethod(context => new PawnInstanceBuilder(
                    context.Container, _pawnParent, _catalog.Prefabs, Container.Resolve<IObservable<GameState>>()
                ))
                .AsCached();

            Container.Bind<BoardActions>().AsTransient();
            Container.Bind<PawnActions>().AsTransient();
            Container.Bind<CombatantActions>().AsTransient();
        }
    }
}