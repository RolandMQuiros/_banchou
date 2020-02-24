using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Banchou.Pawn;

namespace Banchou.Board {
    public class BoardInstaller : MonoInstaller {
        [SerializeField]private PawnCatalog _catalog = null;
        [SerializeField]private Transform _pawnParent = null;

        public override void InstallBindings() {
            var prefabs = _catalog.Prefabs;
            var pawnInstances = new Dictionary<string, GameObject>();

            Container.Bind<CreatePawnInstance>()
                .FromInstance(
                    // Return a Pawn creation function. No factory boilerplate needed
                    (id, prefabKey, position, rotation) => {
                        GameObject prefab;
                        if (prefabs.TryGetValue(prefabKey, out prefab)) {
                            var pawn = Container.InstantiatePrefabForComponent<PawnInstaller>(
                                prefab,
                                position,
                                rotation,
                                _pawnParent,
                                extraArgs: new object[] { id }
                            );
                            pawnInstances[id] = pawn.gameObject;
                            return pawn;
                        }
                        return null;
                    }
                ).WhenInjectedInto<BoardActions>();
            Container.Bind<DestroyPawnInstance>()
                .FromInstance(
                    id => {
                        GameObject instance;
                        if (pawnInstances.TryGetValue(id, out instance)) {
                            GameObject.Destroy(instance);
                        }
                    }
                ).WhenInjectedInto<BoardActions>();
            Container.Bind<Transform>()
                .FromInstance(_pawnParent)
                .WhenInjectedInto<BoardActions>();
            Container.Bind<PawnCatalog>()
                .FromInstance(_catalog)
                .WhenInjectedInto<BoardActions>();
            
            Container.Bind<GetPawnInstance>()
                .FromInstance(id => pawnInstances[id]);
            
            Container.Bind<BoardActions>().AsTransient();
        }
    }

    
    public delegate PawnInstaller CreatePawnInstance(
        string id, string prefabKey, Vector3 position = default, Quaternion rotation = default
    );

    public delegate GameObject GetPawnInstance(string id);

    public delegate void DestroyPawnInstance(string id);
}