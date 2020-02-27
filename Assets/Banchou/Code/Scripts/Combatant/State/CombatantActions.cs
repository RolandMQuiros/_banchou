using System;
using UnityEngine;

namespace Banchou.Combatant {
    namespace StateAction {
        public class CombatantAction {
            public Guid ID;
        }

        public class Hurt : CombatantAction {
            public Guid From;
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
    }

    public class CombatantActions {
        public StateAction.Hurt Hurt(
            Guid pawnID,
            Guid from = default,
            int amount = 0,
            Vector3 push = default
        ) => new StateAction.Hurt { ID = pawnID, From = from, Amount = amount, Push = push};
        public StateAction.Launch Launch(Guid pawnID) => new StateAction.Launch { ID = pawnID };
        public StateAction.Heal Heal(Guid pawnID, int amount) => new StateAction.Heal {
            ID = pawnID,
            Amount = amount
        };
        public StateAction.PushCommand PushCommand(Guid pawnID, Command command) => new StateAction.PushCommand {
            ID = pawnID,
            Command = command
        };
    }
}