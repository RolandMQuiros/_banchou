using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Part {
    public class LockOn : MonoBehaviour {
        private HashSet<Transform> _targets = new HashSet<Transform>();
        public IEnumerable<Transform> Targets => _targets;

        private void OnTriggerEnter(Collider collider) {
            if (collider.GetComponentInChildren<Targettable>()) {
                _targets.Add(collider.transform);
            }
        }

        private void OnTriggerExit(Collider collider) {
            _targets.Remove(collider.transform);
        }

        private void OnDrawGizmos() {
            foreach (var t in _targets) {
                Gizmos.DrawSphere(t.position, 0.5f);
            }
        }
    }
}