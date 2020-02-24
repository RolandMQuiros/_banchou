using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Banchou.Pawn;

namespace Banchou.Board {
    public class PawnInstanceFactory :
        IFactory<string, string, Vector3, Quaternion, PawnInstaller>
    {
        private DiContainer _container;
        private Transform _pawnParent;
        private Dictionary<string, GameObject> _prefabs;
        private Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
        
        public PawnInstanceFactory(
            DiContainer container,
            Transform pawnParent,
            Dictionary<string, GameObject> prefabs
        ) {
            _container = container;
            _pawnParent = pawnParent;
            _prefabs = prefabs;
        }

        public PawnInstaller Create(string id, string prefabKey, Vector3 position, Quaternion rotation) {
            GameObject prefab;
            if (_prefabs.TryGetValue(prefabKey, out prefab)) {
                var instance = _container.InstantiatePrefabForComponent<PawnInstaller>(
                    prefab,
                    position,
                    rotation,
                    _pawnParent,
                    extraArgs: new object[] { id }
                );
                _instances[id] = instance.gameObject;
                return instance;
            }
            return null;
        }

        public void Destroy(string id) {
            GameObject instance;
            if (_instances.TryGetValue(id, out instance)) {
                GameObject.Destroy(instance);
            }
        }
    }
}