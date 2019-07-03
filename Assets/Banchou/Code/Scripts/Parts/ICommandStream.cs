using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Parts {
    public enum Command {
        NeutralLight,
        NeutralLightChargeStart,
        NeutralLightChargeEnd,

        ForwardLight,
        ForwardLightChargeStart,
        ForwardLightChargeEnd,

        SideLight,
        SideLightChargeStart,
        SideLightChargeEnd,

        BackLight,
        BackLightChargeStart,
        BackLightChargeEnd,

        NeutralHeavy,
        NeutralHeavyChargeStart,
        NeutralHeavyChargeEnd,

        SideHeavy,
        SideHeavyChargeStart,
        SideHeavyChargeEnd,

        BackHeavy,
        BackHeavyChargeStart,
        BackHeavyChargeEnd,

        Dodge,
        Jump
    };

    public struct CommandUnit {
        public Command Command;
        public Vector2 Tilt;
    }

    public interface ICommandStream {
        IObservable<CommandUnit> Commands { get; }
    }
}