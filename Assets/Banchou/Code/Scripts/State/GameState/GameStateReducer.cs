using Banchou.Combat.State;

namespace Banchou.State {
    public static class GameStateReducer {
        public static GameState Reduce(in GameState prev, in object action) {
            var hydrate = action as Action.HydrateGameState;
            if (hydrate != null) {
                return new GameState(hydrate.GameState);
            }

            return new GameState(
                battle: BattleReducer.Reduce(prev?.Battle, action)
            );
        }
    }
}