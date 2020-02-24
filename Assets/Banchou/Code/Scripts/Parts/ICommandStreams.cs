using System;
using UnityEngine;

using Banchou.Combatant;

namespace Banchou.Part {
    public interface ICommandStream {
        IObservable<Command> Commands { get; }
        bool IsBuffered(Command command);
    }

    public interface IMovementInput {
        Vector3 MovementDirection { get; }
        bool RotateToMovement { get; }
    }
}