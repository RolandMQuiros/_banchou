using System;
using UnityEngine;

namespace Banchou {
    public class HitVolume : DispatcherBehaviour {
        private string _pawnID;

        public void Inject(string pawnID) {
            _pawnID = pawnID;
        }

        private void OnTriggerEnter(Collider other) {
            var hurt = other.GetComponent<HurtVolume>();
            if (hurt != null) {
                Dispatch(
                    Actions.DamagePawn(
                        _pawnID,
                        from: hurt.PawnID,
                        amount: hurt.BaseDamage
                    )
                );
            }
        }
    }
}
