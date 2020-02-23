using UnityEngine;
using Zenject;

namespace Banchou {
    public class HurtVolume : MonoBehaviour {
        public int BaseDamage;
        [Inject(Id = "PawnID")] public string PawnID { get; private set; }
    }
}