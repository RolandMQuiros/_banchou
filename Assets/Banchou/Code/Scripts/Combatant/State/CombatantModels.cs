using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Combatant {
    public enum Command {
        None, // Always excluded from stream

        NeutralLight,
        NeutralLightCharge,

        PushLight,
        PushLightCharge,

        PullLight,
        PullLightCharge,

        NeutralHeavy,
        NeutralHeavyCharge,

        PushHeavy,
        PushHeavyCharge,

        PullHeavy,
        PullHeavyCharge,

        LightChargeRelease,
        HeavyChargeRelease,

        Dash,
        Dodge,
        Jump,
        Hop
    };

    public class CombatantState {
        public string PawnID;
        public IEnumerable<Command> Commands = Enumerable.Empty<Command>();
        public int Health;
        public Vector3 Push;
        public DateTime Created = DateTime.UtcNow;
        public DateTime Updated = DateTime.UtcNow;
        public CombatantState() { }
        public CombatantState(in CombatantState prev) {
            PawnID = prev?.PawnID;
            Commands = prev?.Commands;
            Health = prev?.Health ?? 0;
            Push = prev?.Push ?? Vector3.zero;
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
    }
}