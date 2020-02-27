using System;
using UnityEngine;
using Zenject;

namespace Banchou {
    public class HurtVolume : MonoBehaviour {
        public int BaseDamage;
        public Guid PawnID { get; private set; }
        
        [Inject]
        public void Construct(Guid pawnID) {
            PawnID = pawnID;
        }
    }
}