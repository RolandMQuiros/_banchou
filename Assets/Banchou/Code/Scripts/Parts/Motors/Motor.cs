using System;
using UnityEngine;

namespace Banchou.Part {
    public interface IMotor {
        void Move(Vector3 velocity);
    }
}