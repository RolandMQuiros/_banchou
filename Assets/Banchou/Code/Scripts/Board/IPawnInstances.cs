using System;
using UnityEngine;

namespace Banchou.Board {
    public interface IPawnInstances {
        GameObject Get(Guid id);
    }
}