using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.FSM {
    public class FSMComment : FSMBehaviour {
        [SerializeField, TextArea]private string _text;
    }
}