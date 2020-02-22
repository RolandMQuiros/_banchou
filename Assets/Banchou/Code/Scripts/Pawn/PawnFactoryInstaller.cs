using System;
using System.Linq;

using UnityEngine;
using UniRx;
using Zenject;

using Banchou.State;

namespace Banchou.Pawn {
    public class PawnFactoryInstaller : MonoInstaller {
        [SerializeField] private PawnCatalog _catalog = null;

        [Inject]
        public void ConnectToStore(
            DiContainer container,
            IObservable<GameState> observeState,
            Dispatcher dispatch
        ) {
            var pawnChanges = observeState
                .Select(state => state.Battle?.Pawns)
                .DistinctUntilChanged()
                .Pairwise();
                
            pawnChanges
                .Select(pair => pair.Current.Except(pair.Previous))
                .Subscribe(toAdd => {
                    
                });
        }

        public override void InstallBindings() {
            var prefabs = _catalog.Prefabs;

            Container.Bind<CreatePawn>()
                .FromMethod(() => {
                    // Get the closest [Project|Scene|GameObject]Context
                    var context = Container.Resolve<Context>();

                    // Return a Pawn creation function. No factory boilerplate needed
                    return (id, prefabKey, position, rotation) => {
                        GameObject prefab;
                        if (prefabs.TryGetValue(prefabKey, out prefab)) {
                            return Container.InstantiatePrefabForComponent<PawnInstaller>(
                                prefab,
                                position,
                                rotation,
                                context.transform,
                                extraArgs: new object[] { id }
                            );
                        }
                        return null;
                    };
                });
        }
    }

    public delegate PawnInstaller CreatePawn(
        string id, string prefabKey, Vector3 position = default, Quaternion rotation = default
    );
}