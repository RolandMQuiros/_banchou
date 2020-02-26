using System;
using UnityEngine;
using Cinemachine;
using Zenject;
using UniRx;

using Banchou.Pawn;
using Banchou.Board;

namespace Banchou {
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CameraGrouper : MonoBehaviour {
        [Inject]
        public void Construct(
            IObservable<GameState> observeState,
            IPawnInstances pawnInstances
        ) {
            var targetGroup = GetComponent<CinemachineTargetGroup>();
            observeState.AddedPawns()
                .SelectMany(pawns => pawns)
                .Where(pawn => pawn.CameraWeight > 0f)
                .Subscribe(pawn => {
                    targetGroup.AddMember(
                        pawnInstances.Get(pawn.ID).transform,
                        pawn.CameraWeight,
                        1f
                    );
                });
            observeState.RemovedPawns()
                .SelectMany(pawns => pawns)
                .Subscribe(pawn => {
                    targetGroup.RemoveMember(
                        pawnInstances.Get(pawn.ID).transform
                    );
                });

        }
    }
}