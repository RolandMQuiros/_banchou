using System;
using UnityEngine;
using UniRx;
using Zenject;

using Banchou.Combatant;

namespace Banchou.Part {
    /// <summary>
    /// Applies changes to a Combatant's state to a Pawn's state machine
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CombatantPawn : MonoBehaviour {
        [Header("Stat FSM Parameters")]
        [SerializeField] private string _healthParameter = "Health";

        [Inject]
        public void Connect(IObservable<GameState> observeState, Dispatcher dispatch, Guid pawnID) {
            var stateMachine = GetComponent<Animator>();
            
            var observePawn = observeState
                .Select(state => state.GetCombatant(pawnID))
                .DistinctUntilChanged();
            
            // Handle health changes
            var healthKey = Animator.StringToHash(_healthParameter);
            observePawn
                .Select(pawn => pawn.Health)
                .Distinct()
                .Subscribe(health => {
                    stateMachine.SetInteger(healthKey, health);
                });
        }
    }
}