using UnityEngine;
using Sirenix.OdinInspector;

using Banchou.Board;

namespace Banchou {
    [CreateAssetMenu(menuName = "Banchou/Game State")]
    public class GameState : SerializedScriptableObject {
        public BoardState Board = new BoardState();

        public GameState(
            GameState prev = null,
            BoardState board = null
        ) {
            Board = board ?? prev?.Board ?? new BoardState();
        }
    }
}