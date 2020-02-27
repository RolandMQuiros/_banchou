using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

using Banchou.Pawn;

namespace Banchou.Board {
    public class PawnInstance {
        public Guid ID { get; private set; }
        public GameObject GameObject { get; private set; }

        public PawnInstance(Guid id, GameObject gameObject) {
            ID = id;
            GameObject = gameObject;
        }

        public class Factory : PlaceholderFactory<PawnState, PawnInstance> { }
    }

    public class PawnInstanceFactory : IFactory<PawnState, PawnInstance> {
        private DiContainer _container;
        private Dictionary<string, GameObject> _prefabs;
        private Transform _pawnParent;
        
        public PawnInstanceFactory(
            DiContainer container,
            Dictionary<string, GameObject> prefabs,
            Transform pawnParent
        ) {
            _container = container;
            _prefabs = prefabs;
            _pawnParent = pawnParent;
        }

        public PawnInstance Create(PawnState pawnState) {
            GameObject prefab;
            if (_prefabs.TryGetValue(pawnState.PrefabKey, out prefab)) {
                var subContainer = _container.CreateSubContainer();
                subContainer.BindInstance(pawnState.ID);
                var instance = subContainer.InstantiatePrefab(
                    prefab,
                    new GameObjectCreationParameters {
                        Name = pawnState.DisplayName,
                        ParentTransform = _pawnParent,
                        Position = pawnState.InitialPosition
                    }
                );
                return new PawnInstance(pawnState.ID, instance);
            }
            return null;
        }
    }

    public class PawnInstanceBuilder : IDisposable, IPawnInstances {
        private Dictionary<Guid, GameObject> _instances = new Dictionary<Guid, GameObject>();
        private IDisposable _createStream;
        private IDisposable _destroyStream;
        
        public PawnInstanceBuilder(
            PawnInstance.Factory factory,
            IObservable<GameState> observeState
        ) {
            _createStream = observeState.EachAddedPawn().Subscribe(p => {
                _instances[p.ID] = factory.Create(p).GameObject;
            });
            _destroyStream = observeState.EachRemovedPawn().Subscribe(p => {
                GameObject instance;
                if (_instances.TryGetValue(p.ID, out instance)) {
                    GameObject.Destroy(instance);
                }
            });
        }

        public void Dispose() {
            _createStream.Dispose();
            _destroyStream.Dispose();
        }

        public GameObject Get(Guid id) {
            GameObject instance;
            if (_instances.TryGetValue(id, out instance)) {
                return instance;
            }
            return null;
        }
    }
}