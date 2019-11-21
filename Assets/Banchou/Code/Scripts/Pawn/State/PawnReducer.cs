using System.Linq;
using UnityEngine;

namespace Banchou.Pawn.State {
    public static class PawnReducer {
        public static PawnState Reduce(in PawnState prev, in object action) =>
            ApplyDamage(prev, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static PawnState ApplyDamage(in PawnState prev, in object action) {
            var heal = action as Action.HealPawn;
            if (heal != null) {
                return new PawnState(
                    prev,
                    health: prev.Health + heal.Amount
                );
            }

            var damage = action as Action.DamagePawn;
            if (damage != null) {
                return new PawnState(
                    prev,
                    health: prev.Health - damage.Amount,
                    push: damage.Push
                );
            }

            var pushed = action as Action.PawnPushed;
            if (pushed != null) {
                return new PawnState(
                    prev,
                    push: Vector3.zero
                );
            }
            
            return null;
        }

        private static PawnState ApplyCommands(in PawnState prev, in object action) {
            var push = action as Action.PushPawnCommand;
            if (push != null) {
                return new PawnState(
                    prev,
                    commands: prev.Commands.Append(push.Command)
                );
            }

            var pop = action as Action.PopPawnCommand;
            if (pop != null) {
                return new PawnState(
                    prev,
                    commands: prev.Commands.Reverse()
                                           .Skip(1)
                                           .Reverse()
                );
            }

            var clear = action as Action.ClearPawnCommands;
            if (clear != null) {
                return new PawnState(
                    prev,
                    commands: Enumerable.Empty<PawnState.IQueuedCommand>()
                );
            }

            return null;
        }
    }
}