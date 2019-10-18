﻿using System.Linq;

namespace Banchou {
    public static partial class Reducers {
        private static Pawn PawnReducer(in Pawn prev, in object action) =>
            ApplyDamage(prev, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static Pawn ApplyDamage(in Pawn prev, in object action) {
            var heal = action as Action.Heal;
            if (heal != null) {
                return new Pawn(
                    prev,
                    health: prev.Health + heal.Amount
                );
            }

            var damage = action as Action.Damage;
            if (damage != null) {
                return new Pawn(
                    prev,
                    health: prev.Health - heal.Amount
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