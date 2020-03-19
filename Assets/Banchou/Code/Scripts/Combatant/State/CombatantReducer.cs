using System.Linq;
using UnityEngine;

namespace Banchou.Combatant {
    public static class CombatantReducer {
        public static CombatantState Reduce(in CombatantState prev, in object action) =>
            ApplyDamage(prev, action) ??
            ApplyCommands(prev, action) ??
            prev;

        private static CombatantState ApplyDamage(in CombatantState prev, in object action) {
            var heal = action as StateAction.Heal;
            if (heal != null) {
                return new CombatantState(prev) {
                    Health = prev.Health + heal.Amount
                };
            }

            var damage = action as StateAction.Hurt;
            if (damage != null) {
                return new CombatantState(prev) {
                    Health = prev.Health - damage.Amount,
                    Launch = new Launch {
                        Force = damage.Launch,
                        When = damage.When
                    }
                };
            }

            var pushed = action as StateAction.Launch;
            if (pushed != null) {
                return new CombatantState(prev) {
                    Launch = new Launch {
                        Force = pushed.Force,
                        When = pushed.When
                    }
                };
            }
            
            return null;
        }

        private static CombatantState ApplyCommands(in CombatantState prev, in object action) {
            var push = action as StateAction.PushCommand;
            if (push != null) {
                return new CombatantState(prev) {
                    Command = push.Command
                };
            }
            return null;
        }
    }
}