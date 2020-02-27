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

    [Flags]
    public enum Team {
        Neutral,
        Prop
    }


    public class CombatantState {
        public Guid PawnID = Guid.Empty;
        public Team Team = Team.Neutral;
        public int Health = -1;
        public Vector3 Push;
        public Command Command;
        public DateTime Created = DateTime.UtcNow;
        public DateTime Updated = DateTime.UtcNow;
        public CombatantState() { }
        public CombatantState(in CombatantState prev) {
            PawnID = prev?.PawnID ?? Guid.Empty;
            Command = prev?.Command ?? Command.None;
            Health = prev?.Health ?? -1;
            Push = prev?.Push ?? Vector3.zero;
            Created = prev?.Created ?? DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }
    }
}