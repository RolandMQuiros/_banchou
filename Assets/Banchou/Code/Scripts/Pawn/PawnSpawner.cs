using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace Banchou.Pawn {
    public class PawnSpawner : MonoBehaviour {
        private Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();

        [Inject]
        public void Construct(
            DiContainer container,
            IObservable<GameState> observeState,
            Dispatcher dispatch,
            CreatePawn createPawn
        ) {
            var pawnChanges = observeState
                .Select(state => state.GetPawns())
                .DistinctUntilChanged()
                .Pairwise();
                
            pawnChanges
                .SelectMany(pair => pair.Current.Except(pair.Previous))
                .Subscribe(SpawnPawn);
            
            pawnChanges
                .SelectMany(pair => pair.Previous.Except(pair.Current))
                .Subscribe(DestroyPawn);

            void SpawnPawn(PawnState pawnState) {
                GameObject existing;
                if (_instances.TryGetValue(pawnState.ID, out existing)) {
                    throw new Exception($"Cannot spawn Pawn \"{pawnState.ID}\", a GameObject \"{existing.name}\" is already attached");
                }
                var instance = _instances[pawnState.ID] = createPawn(
                    pawnState.ID,
                    pawnState.PrefabKey,
                    position: pawnState.InitialPosition,
                    parent: transform
                ).gameObject;
                instance.name = pawnState.DisplayName;
            }

            void DestroyPawn(PawnState pawnState) {
                GameObject existing;
                if (!_instances.TryGetValue(pawnState.ID, out existing)) {
                    throw new Exception($"Cannot remove Pawn \"{pawnState.ID}\", an associated GameObject does not exist");
                }
                _instances.Remove(pawnState.ID);
                GameObject.Destroy(existing);
            }
        }
    }
}