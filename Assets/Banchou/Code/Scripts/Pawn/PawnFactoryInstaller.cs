using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Banchou.Pawn {
    public class PawnFactoryInstaller : MonoInstaller {
        [SerializeField] private PawnCatalog _catalog = null;

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