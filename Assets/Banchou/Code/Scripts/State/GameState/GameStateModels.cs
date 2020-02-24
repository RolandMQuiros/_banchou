using Banchou.Board;

namespace Banchou {
    public class GameState {
        public BoardState Board = new BoardState();

        public GameState(GameState prev = null) {
            Board = prev?.Board ?? new BoardState();
        }
    }
}