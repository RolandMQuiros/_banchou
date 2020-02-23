using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

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
                });
        }
    }
}
