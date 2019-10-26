using Banchou.State.Model;

namespace Banchou {
    public class GameState {
        public Battle Battle;

        public GameState(
            GameState prev = null,
            Battle battle = null
        ) {
            Battle = battle ?? prev?.Battle ?? new Battle();
        }
    }
}