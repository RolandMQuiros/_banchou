using System;
using UnityEngine;

namespace Banchou {
    public class HurtVolume : MonoBehaviour {
        public int BaseDamage;
        public string PawnID { get; private set; }
        public void Inject(string pawnID) {
            PawnID = pawnID;
        }
    }
}