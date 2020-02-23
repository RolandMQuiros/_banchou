using Banchou.Board;

namespace Banchou {
    public static class GameStateReducer {
        public static GameState Reduce(in GameState prev, in object action) {
            var hydrate = action as StateAction.HydrateGameState;
            if (hydrate != null) {
                return new GameState(hydrate.GameState);
            }

            return new GameState(prev) {
                Board = BoardReducer.Reduce(prev?.Board, action)
            };
        }
    }
}