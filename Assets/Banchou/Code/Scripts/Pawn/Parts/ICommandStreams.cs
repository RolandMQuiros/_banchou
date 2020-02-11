using System;
using UnityEngine;

namespace Banchou.Part {
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

    public interface ICommandStream {
        IObservable<Command> Commands { get; }
        bool IsBuffered(Command command);
    }

    public interface IMovementInput {
        Vector3 MovementDirection { get; }
        bool RotateToMovement { get; }
    }
}