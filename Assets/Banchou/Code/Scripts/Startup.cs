using UnityEngine;
using Banchou.Board;
using Zenject;

namespace Banchou {
    public class Startup : MonoBehaviour {
        [Inject]
        public void Construct(
            Dispatcher dispatch,
            BoardActions actions
        ) {
            dispatch(actions.AddPawn(
                prefabKey: "Isaac",
                displayName: "Isaac",
                cameraWeight: 1f,
                position: new Vector3(10f, 1f, -3f)
            ));

            dispatch(actions.AddPawn(
                prefabKey: "TestCylinder",
                displayName: "TestEnemy1",
                cameraWeight: 1f,
                position: new Vector3(15f, 1f, -4f)
            ));
        }
    }
}
