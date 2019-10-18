using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.FSM {
    public class FSMComment : FSMBehaviour {
        [SerializeField, TextArea(maxLines: 15, minLines: 5)]
        private string _text;
    }
}