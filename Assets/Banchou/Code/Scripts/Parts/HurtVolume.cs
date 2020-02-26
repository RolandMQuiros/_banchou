using UnityEngine;
using Zenject;

namespace Banchou {
    public class HurtVolume : MonoBehaviour {
        public int BaseDamage;
        public string PawnID { get; private set; }
        public void Construct(
            [Inject(Id = "PawnID")] string pawnID
        ) {
            PawnID = pawnID;
        }
    }
}