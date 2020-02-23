using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou.Part {
    public class ViewInputAxis : MonoBehaviour {
        [SerializeField] private Transform _camera = null;
        [SerializeField] private Transform _body = null;
        public Vector3 Project(Vector2 controlAxes) => Project(_camera.forward, _camera.right, _body.up, controlAxes);
        public static Vector3 Project(Vector3 viewForward, Vector3 viewRight, Vector3 planeNormal, Vector2 controlAxes) {
            var forward = Vector3.ProjectOnPlane(viewForward, planeNormal).normalized;
            var right = Vector3.ProjectOnPlane(viewRight, planeNormal).normalized;
            return right * controlAxes.x + forward * controlAxes.y;
        }
    }
}