using System.Linq;
using UnityEngine;

namespace Banchou.Pawn {
    public static class PawnReducer {
        public static PawnState Reduce(in PawnState prev, in object action) =>
            ApplyDamage(prev, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static PawnState ApplyDamage(in PawnState prev, in object action) {
            var heal = action as StateAction.HealPawn;
            if (heal != null) {
                return new PawnState(
                    prev,
                    health: prev.Health + heal.Amount
                );
            }

            var damage = action as StateAction.DamagePawn;
            if (damage != null) {
                return new PawnState(
                    prev,
                    health: prev.Health - damage.Amount,
                    push: damage.Push
                );
            }

            var pushed = action as StateAction.PawnPushed;
            if (pushed != null) {
                return new PawnState(
                    prev,
                    push: Vector3.zero
                );
            }
            
            return null;
        }

        private static PawnState ApplyCommands(in PawnState prev, in object action) {
            var push = action as StateAction.PushPawnCommand;
            if (push != null) {
                return new PawnState(
                    prev,
                    commands: prev.Commands.Append(push.Command)
                );
            }

            var pop = action as StateAction.PopPawnCommand;
            if (pop != null) {
                return new PawnState(
                    prev,
                    commands: prev.Commands.Reverse()
                                           .Skip(1)
                                           .Reverse()
                );
            }

            var clear = action as StateAction.ClearPawnCommands;
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