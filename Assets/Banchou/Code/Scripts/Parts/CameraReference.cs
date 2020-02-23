using UnityEngine;

namespace Banchou.Part {
    public class CameraReference : MonoBehaviour {
        [SerializeField] private Transform _camera = null;
        public Transform Camera => _camera;
    }
}
