using System.Linq;
using UnityEngine;

namespace Banchou.Combatant {
    public static class CombatantReducer {
        public static CombatantState Reduce(in CombatantState prev, in object action) =>
            ApplyDamage(prev, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static CombatantState ApplyDamage(in CombatantState prev, in object action) {
            var heal = action as StateAction.HealPawn;
            if (heal != null) {
                return new CombatantState(prev) {
                    Health = prev.Health + heal.Amount
                };
            }

            var damage = action as StateAction.DamagePawn;
            if (damage != null) {
                return new CombatantState(prev) {
                    Health = prev.Health - damage.Amount,
                    Push = damage.Push
                };
            }

            var pushed = action as StateAction.Launched;
            if (pushed != null) {
                return new CombatantState(prev) {
                    Push = Vector3.zero
                };
            }
            
            return null;
        }

        private static CombatantState ApplyCommands(in CombatantState prev, in object action) {
            var push = action as StateAction.PushPawnCommand;
            if (push != null) {
                return new CombatantState(prev) {
                    Commands = prev.Commands.Append(push.Command)
                };
            }

            var pop = action as StateAction.PopPawnCommand;
            if (pop != null) {
                return new CombatantState(prev) {
                    Commands = prev.Commands
                        .Reverse()
                        .Skip(1)
                        .Reverse()
                };
            }

            var clear = action as StateAction.ClearPawnCommands;
            if (clear != null) {
                return new CombatantState(prev) {
                    Commands = Enumerable.Empty<Command>()
                };
            }

            return null;
        }
    }
}