using UnityEngine;

namespace Banchou.Combatant {
    namespace StateAction {
        public class CombatantAction {
            public string ID;
        }

        public class Hurt : CombatantAction {
            public string From;
            public int Amount;
            public Vector3 Push;
        }

        public class Launch : CombatantAction { }

        public class Heal : CombatantAction {
            public int Amount;
        }

        public class PushCommand : CombatantAction {
            public Command Command;
        }

        public class PopCommand : CombatantAction { }
        public class ClearCommands : CombatantAction { }
    }

    public class CombatantActions {
        public StateAction.Hurt Hurt(
            string pawnID,
            string from = default,
            int amount = 0,
            Vector3 push = default
        ) => new StateAction.Hurt { ID = pawnID, From = from, Amount = amount, Push = push};
        public StateAction.Launch Launch(string pawnID) => new StateAction.Launch { ID = pawnID };
        public StateAction.Heal Heal(string pawnID, int amount) => new StateAction.Heal {
            ID = pawnID,
            Amount = amount
        };
    }
}