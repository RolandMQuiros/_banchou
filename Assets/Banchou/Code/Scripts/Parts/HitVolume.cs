﻿using System;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

using Banchou.Combatant;

namespace Banchou.Part {
    public class HitVolume : MonoBehaviour {
        [Inject]
        public void Construct(
            Dispatcher dispatch,
            CombatantActions actions,
            Guid pawnID
        ) {
            this.OnTriggerEnterAsObservable()
                .Select(other => other.GetComponent<HurtVolume>())
                .Where(hurt => hurt != null)
                .Subscribe(hurt => {
                    dispatch(
                        actions.Hurt(
                            pawnID,
                            from: hurt.PawnID,
                            amount: hurt.BaseDamage
                        )
                    );
                });
        }
    }
}
