using UnityEngine;
using Sirenix.OdinInspector;

using Banchou.Combat.State;

namespace Banchou.State {
    [CreateAssetMenu(menuName = "Banchou/Game State")]
    public class GameState : SerializedScriptableObject {
        public BattleState Battle = new BattleState();

        public GameState(
            GameState prev = null,
            BattleState battle = null
        ) {
            Battle = battle ?? prev?.Battle ?? new BattleState();
        }
    }
}