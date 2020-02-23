using UnityEngine;

namespace Banchou.Pawn {
    namespace StateAction {
        public class PawnAction {
            public string PawnID;
        }

        public class DamagePawn : PawnAction {
            public string From;
            public int Amount;
            public Vector3 Push;
        }

        public class PawnPushed : PawnAction { }

        public class HealPawn : PawnAction {
            public int Amount;
        }

        public class PushPawnCommand : PawnAction {
            public PawnState.IQueuedCommand Command;
        }

        public class PopPawnCommand : PawnAction { }
        public class ClearPawnCommands : PawnAction { }
    }

    public class PawnActions {
        public StateAction.DamagePawn DamagePawn(
            string pawnID,
            string from = default,
            int amount = 0,
            Vector3 push = default
        ) => new StateAction.DamagePawn { PawnID = pawnID, From = from, Amount = amount, Push = push};
        public StateAction.PawnPushed PawnPushed(string pawnID) => new StateAction.PawnPushed { PawnID = pawnID };
        public StateAction.HealPawn HealPawn(string pawnID, int amount) => new StateAction.HealPawn {
            PawnID = pawnID,
            Amount = amount
        };
    }
}