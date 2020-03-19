using System;
using UnityEngine;
using UniRx;
using Zenject;

using Banchou.Combatant;

namespace Banchou.FSM {
    [RequireComponent(typeof(Rigidbody))]
    public class Launchable : FSMBehaviour {
        [SerializeField] private string _launchMagnitudeParameter = "Launch Magnitude";
        [SerializeField] private string _launchXParameter = "Launch X";
        [SerializeField] private string _launchYParameter = "Launch Y";
        [SerializeField] private string _launchZParameter = "Launch Z";

        [Inject]
        public void Construct(
            Guid pawnID,
            IObservable<GameState> observeState,
            Rigidbody rigidBody,
            Animator stateMachine
        ) {
            var launchMagKey = Animator.StringToHash(_launchMagnitudeParameter);
            var launchXKey = Animator.StringToHash(_launchXParameter);
            var launchYKey = Animator.StringToHash(_launchYParameter);
            var launchZKey = Animator.StringToHash(_launchZParameter);

            observeState
                .Select(state => state.GetCombatant(pawnID))
                .Where(pawn => pawn != null)
                .Select(pawn => pawn.Launch)
                .DistinctUntilChanged()
                .Subscribe(launch => {
                    rigidBody.AddForce(launch.Force, ForceMode.VelocityChange);

                    var launchDirection = launch.Force.normalized;
                    if (launchMagKey != 0) { stateMachine.SetFloat(launchMagKey, launch.Force.magnitude); }
                    if (launchXKey != 0) { stateMachine.SetFloat(launchXKey, launchDirection.x); }
                    if (launchYKey != 0) { stateMachine.SetFloat(launchYKey, launchDirection.y); }
                    if (launchZKey != 0) { stateMachine.SetFloat(launchZKey, launchDirection.z); }
                });
        }
    }
}