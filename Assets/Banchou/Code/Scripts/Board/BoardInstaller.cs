using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

using Banchou.Pawn;

namespace Banchou.Board {
    public delegate GameObject GetPawnInstance(string id);

    public class BoardInstaller : MonoInstaller {
        [SerializeField]private PawnCatalog _catalog = null;
        [SerializeField]private Transform _pawnParent = null;

        private Dictionary<string, GameObject> _prefabs;
        private Dictionary<string, GameObject> _pawnInstances = new Dictionary<string, GameObject>();

        [Inject]
        public void Construct(IObservable<GameState> observeState) {
            _prefabs = _catalog.Prefabs;

            if (_pawnParent == null) {
                _pawnParent = (new GameObject("Pawns/")).transform;
                _pawnParent.parent = transform;
            }

            observeState.EachAddedPawn().Subscribe(CreatePawnInstance);
            observeState.EachRemovedPawn().Subscribe(p => DestroyPawnInstance(p.ID));
        }

        public override void InstallBindings() {
            Container.Bind<GetPawnInstance>().FromInstance(GetPawnInstance);
            Container.Bind<BoardActions>().AsTransient();
        }

        private void CreatePawnInstance(PawnState pawnState) {
            GameObject prefab;
            if (_prefabs.TryGetValue(pawnState.PrefabKey, out prefab)) {
                var pawn = Container.InstantiatePrefabForComponent<PawnInstaller>(
                    prefab,
                    pawnState.InitialPosition,
                    Quaternion.identity,
                    _pawnParent,
                    extraArgs: new object[] { pawnState.ID }
                );
                _pawnInstances[pawnState.ID] = pawn.gameObject;
            }
        }

        private void DestroyPawnInstance(string id) {
            GameObject instance;
            if (_pawnInstances.TryGetValue(id, out instance)) {
                GameObject.Destroy(instance);
            }
        }

        private GameObject GetPawnInstance(string id) {
            return _pawnInstances[id];
        }
    }
}