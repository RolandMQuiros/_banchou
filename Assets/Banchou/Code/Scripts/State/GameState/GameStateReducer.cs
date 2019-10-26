namespace Banchou.State {
    public static partial class Reducers {
        public static GameState GameStateReducer(in GameState prev, in object action) {
            var hydrate = action as Action.HydrateGameState;
            if (hydrate != null) {
                return new GameState(hydrate.GameState);
            }

            return new GameState(
                battle: BattleReducer(prev?.Battle, action)
            );
        }
    }
}