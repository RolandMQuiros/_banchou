using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Combat {
    public class CombatState {
        public readonly IEnumerable<Combatant> Left;
        public readonly IEnumerable<Combatant> Right;

        public CombatState(
            CombatState prev = null,
            IEnumerable<Combatant> left = null,
            IEnumerable<Combatant> right = null
        ) {
            Left = prev?.Left ?? left ?? Enumerable.Empty<Combatant>();
            Right = prev?.Right ?? right ?? Enumerable.Empty<Combatant>();
        }
    }
}