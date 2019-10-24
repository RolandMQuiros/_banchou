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
        [Header("Stat FSM Parameters")]
        [SerializeField] private string _healthParameter = "Health";

        [Header("Push FSM Parameters")]
        [SerializeField] private string _pushMagnitudeParameter = "Push Magnitude";
        [SerializeField] private string _pushXParameter = "Push X";
        [SerializeField] private string _pushYParameter = "Push Y";
        [SerializeField] private string _pushZParameter = "Push Z";

        public void Inject(IObservable<State> observeState, Func<object, object> dispatch) {
            var stateMachine = GetComponent<Animator>();
            
            var observePawn = observeState
                .Select(state => state.GetPawn(PawnID))
                .Distinct();
            
            // Handle health changes
            var healthKey = Animator.StringToHash(_healthParameter);
            observePawn
                .Select(pawn => pawn.Health)
                .Distinct()
                .Subscribe(health => {
                    stateMachine.SetInteger(healthKey, health);
                });
            
            // Handle push forces
            var pushMagKey = Animator.StringToHash(_pushMagnitudeParameter);
            var pushXKey = Animator.StringToHash(_pushXParameter);
            var pushYKey = Animator.StringToHash(_pushYParameter);
            var pushZKey = Animator.StringToHash(_pushZParameter);
            observePawn
                .Select(pawn => pawn.Push)
                .Where(push => push != Vector3.zero)
                .Distinct()
                .Subscribe(push => {
                    var pushDirection = push.normalized;
                    stateMachine.SetFloat(pushMagKey, push.magnitude);
                    stateMachine.SetFloat(pushXKey, pushDirection.x);
                    stateMachine.SetFloat(pushYKey, pushDirection.y);
                    stateMachine.SetFloat(pushZKey, pushDirection.z);
                });
        }
    }
}