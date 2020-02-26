using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

using Banchou.Pawn;

namespace Banchou.Board {
    public class PawnInstanceBuilder : IPawnInstances {
        private Transform _pawnParent = null;
        private DiContainer _container;
        private Dictionary<string, GameObject> _prefabs;
        private Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
        
        public PawnInstanceBuilder(
            DiContainer container,
            Transform pawnParent,
            Dictionary<string, GameObject> prefabs,
            IObservable<GameState> observeState
        ) {
            _container = container;
            _prefabs = prefabs;
            _pawnParent = pawnParent;

            observeState.EachAddedPawn().Subscribe(CreatePawnInstance);
            observeState.EachRemovedPawn().Subscribe(p => DestroyPawnInstance(p.ID));
        }

        private void CreatePawnInstance(PawnState pawnState) {
            GameObject prefab;
            if (_prefabs.TryGetValue(pawnState.PrefabKey, out prefab)) {
                var pawn = _container.InstantiatePrefabForComponent<PawnInstaller>(
                    prefab,
                    pawnState.InitialPosition,
                    Quaternion.identity,
                    _pawnParent,
                    extraArgs: new object[] { pawnState.ID }
                );
                _instances[pawnState.ID] = pawn.gameObject;
            }
        }

        private void DestroyPawnInstance(string id) {
            GameObject instance;
            if (_instances.TryGetValue(id, out instance)) {
                GameObject.Destroy(instance);
            }
        }

        public GameObject Get(string id) {
            GameObject instance;
            if (_instances.TryGetValue(id, out instance)) {
                return instance;
            }
            return null;
        }
    }
}