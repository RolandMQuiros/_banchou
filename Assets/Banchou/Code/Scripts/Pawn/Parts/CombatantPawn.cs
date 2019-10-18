using System;
using UnityEngine;
using UniRx;

namespace Banchou.Part {
    /// <summary>
    /// Applies changes to a Combatant's state to a Pawn's state machine
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CombatantPawn : MonoBehaviour, IStoreConnector {
        public string PawnID { get; set; }
        public void Inject(IObservable<State> observeState, Func<object, object> dispatch) {
            var stateMachine = GetComponent<Animator>();
            
            var observePawn = observeState
                .Select(state => state.GetPawn(PawnID))
                .Distinct();
            
            // Handle health changes
            var healthKey = Animator.StringToHash("Health");
            observePawn
                .Select(pawn => pawn.Health)
                .Distinct()
                .Subscribe(health => {
                    stateMachine.SetInteger(healthKey, health);
                });
        }
    }
}