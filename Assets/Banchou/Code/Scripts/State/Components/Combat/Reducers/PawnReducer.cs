using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Banchou {
    public static partial class Reducers {
        private static Pawn PawnReducer(in Pawn prev, in Dictionary<string, Pawn> pawns, in object action) =>
            ApplyDamage(prev, pawns, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static Pawn ApplyDamage(in Pawn prev, in Dictionary<string, Pawn> pawns, in object action) {
            var heal = action as Action.HealPawn;
            if (heal != null) {
                return new Pawn(
                    prev,
                    health: prev.Health + heal.Amount
                );
            }

            var damage = action as Action.DamagePawn;
            if (damage != null) {
                var other = pawns.Get(damage.From);
                if (other == null || other.Team != prev.Team) {
                    return new Pawn(
                        prev,
                        health: prev.Health - damage.Amount,
                        push: damage.Push
                    );
                }
            }

            var pushed = action as Action.PawnPushed;
            if (pushed != null) {
                return new Pawn(
                    prev,
                    push: Vector3.zero
                );
            }
            
            return null;
        }

        private static Pawn ApplyCommands(in Pawn prev, in object action) {
            var push = action as Action.PushPawnCommand;
            if (push != null) {
                return new Pawn(
                    prev,
                    commands: prev.Commands.Append(push.Command)
                );
            }

            var pop = action as Action.PopPawnCommand;
            if (pop != null) {
                return new Pawn(
                    prev,
                    commands: prev.Commands.Reverse()
                                           .Skip(1)
                                           .Reverse()
                );
            }

            var clear = action as Action.ClearPawnCommands;
            if (clear != null) {
                return new Pawn(
                    prev,
                    commands: Enumerable.Empty<Pawn.IQueuedCommand>()
                );
            }

            return null;
        }
    }
}