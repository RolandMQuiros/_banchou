using UnityEngine;
using Sirenix.OdinInspector;

namespace Banchou {
    [CreateAssetMenu(menuName = "Banchou/Game State Asset")]
    public class GameStateAsset : SerializedScriptableObject{
        public GameState GameState = new GameState();
    }
}