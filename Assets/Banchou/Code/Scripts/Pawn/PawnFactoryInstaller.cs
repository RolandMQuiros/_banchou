using UnityEngine;
using Zenject;

namespace Banchou.Pawn {
    public class PawnFactoryInstaller : MonoInstaller {
        [SerializeField] private PawnCatalog _catalog = null;
        
        public override void InstallBindings() {
            var prefabs = _catalog.Prefabs;

            Container.Bind<CreatePawn>()
                .FromInstance(
                    // Return a Pawn creation function. No factory boilerplate needed
                    (id, prefabKey, position, rotation, parent) => {
                        GameObject prefab;
                        if (prefabs.TryGetValue(prefabKey, out prefab)) {
                            return Container.InstantiatePrefabForComponent<PawnInstaller>(
                                prefab,
                                position,
                                rotation,
                                parent,
                                extraArgs: new object[] { id }
                            );
                        }
                        return null;
                    }
                ).WhenInjectedInto<PawnSpawner>();
        }
    }

    public delegate PawnInstaller CreatePawn(
        string id, string prefabKey, Vector3 position = default, Quaternion rotation = default, Transform parent = null
    );
}