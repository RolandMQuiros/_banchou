using UnityEngine;

namespace Banchou.Action {
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
        public Pawn.IQueuedCommand Command;
    }

    public class PopPawnCommand : PawnAction { }
    public class ClearPawnCommands : PawnAction { }
}

namespace Banchou {
    public static partial class Actions {
        public static Action.DamagePawn DamagePawn(
            string pawnID,
            string from = default,
            int amount = 0,
            Vector3 push = default
        ) => new Action.DamagePawn { PawnID = pawnID, From = from, Amount = amount, Push = push};
        public static Action.PawnPushed PawnPushed(string pawnID) => new Action.PawnPushed { PawnID = pawnID };
        public static Action.HealPawn HealPawn(string pawnID, int amount) => new Action.HealPawn {
            PawnID = pawnID,
            Amount = amount
        };
    }
}