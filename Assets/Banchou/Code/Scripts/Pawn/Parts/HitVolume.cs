using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

using Banchou.Pawn.State;

namespace Banchou.Pawn {
    public class HitVolume : MonoBehaviour {
        [Inject]
        public void Construct(
            Dispatcher dispatch,
            PawnActions actions,
            [Inject(Id = "PawnID")] string pawnID
        ) {
            this.OnTriggerEnterAsObservable()
                .Select(other => other.GetComponent<HurtVolume>())
                .Where(hurt => hurt != null)
                .Subscribe(hurt => {
                    dispatch(
                        actions.DamagePawn(
                            pawnID,
                            from: hurt.PawnID,
                            amount: hurt.BaseDamage
                        )
                    );
                }).AddTo(this);
        }
    }
}
