using UnityEngine;
using Sirenix.OdinInspector;

using Banchou.Board;

namespace Banchou {
    public class GameState {
        public BoardState Board = new BoardState();

        public GameState(GameState prev = null) {
            Board = prev?.Board ?? new BoardState();
        }
    }

    [CreateAssetMenu(menuName = "Banchou/Game State Instance")]
    public class GameStateInstance : SerializedScriptableObject{
        public GameState GameState;
    }
}