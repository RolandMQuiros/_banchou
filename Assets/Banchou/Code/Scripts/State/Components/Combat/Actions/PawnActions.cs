namespace Banchou.Action {
    public class PawnAction {
        public string ID;
    }

    public class Damage : PawnAction {
        public int Amount;
    }

    public class Heal : PawnAction {
        public int Amount;
    }

    public class PushPawnCommand : PawnAction {
        public Pawn.IQueuedCommand Command;
    }

    public class PopPawnCommand : PawnAction { }
    public class ClearPawnCommands : PawnAction { }
}