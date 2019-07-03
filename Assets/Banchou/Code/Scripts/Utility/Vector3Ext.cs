using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Banchou {
    public static class Vector3Ext {
        /// <summary>
        /// Projects an input vector to a view
        /// </summary>
        /// <param name="controlAxes">The input axes to project. X is projected along the viewRight vector, Y along the viewForward</param>
        /// <param name="viewForward">The view's forward direction vector. <c>controlAxes'</c> Y axis is projected along this.</param>
        /// <param name="viewRight">The view's right direction vector. <c>controlAxes'</c> X axis is projected along this</param>
        /// <param name="planeNormal">The up vector of the plane to project upon</param>
        /// <returns></returns>
        public static Vector3 ProjectInputFromView(Vector2 controlAxes, Vector3 viewForward, Vector3 viewRight, Vector3 planeNormal) {
            var forward = Vector3.ProjectOnPlane(viewForward, planeNormal).normalized;
            var right = Vector3.ProjectOnPlane(viewRight, planeNormal).normalized;
            return right * controlAxes.x + forward * controlAxes.y;
        }
    }
}