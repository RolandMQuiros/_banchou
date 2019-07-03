using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Part {
    public class GroundedVolume : MonoBehaviour {
        [SerializeField] private LayerMask _layers = new LayerMask();
        public bool IsGrounded => _detected > 0;
        private int _detected = 0;
        private int _groundedHash;

        private void OnTriggerEnter(Collider collider) {
            if (((1 << collider.gameObject.layer) & _layers.value) != 0) {
                _detected++;
            }
        }

        private void OnTriggerExit(Collider collider) {
            if (((1 << collider.gameObject.layer) & _layers.value) != 0) {
                _detected--;
            }
        }
    }
}
