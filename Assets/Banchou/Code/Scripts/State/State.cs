using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou {
    public class State {
        public Battle Battle;

        public State(
            State prev = null,
            Battle battle = null
        ) {
            Battle = battle ?? prev?.Battle ?? new Battle();
        }
    }
}