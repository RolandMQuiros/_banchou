using UnityEngine;

namespace Banchou.Combatant {
    namespace StateAction {
        public class CombatantAction {
            public string ID;
        }

        public class DamagePawn : CombatantAction {
            public string From;
            public int Amount;
            public Vector3 Push;
        }

        public class Launched : CombatantAction { }

        public class HealPawn : CombatantAction {
            public int Amount;
        }

        public class PushPawnCommand : CombatantAction {
            public Command Command;
        }

        public class PopPawnCommand : CombatantAction { }
        public class ClearPawnCommands : CombatantAction { }
    }

    public class CombatantActions {
        public StateAction.DamagePawn DamagePawn(
            string pawnID,
            string from = default,
            int amount = 0,
            Vector3 push = default
        ) => new StateAction.DamagePawn { ID = pawnID, From = from, Amount = amount, Push = push};
        public StateAction.Launched Launched(string pawnID) => new StateAction.Launched { ID = pawnID };
        public StateAction.HealPawn HealPawn(string pawnID, int amount) => new StateAction.HealPawn {
            ID = pawnID,
            Amount = amount
        };
    }
}