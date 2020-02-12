using UnityEngine;
using Zenject;

using Banchou.Pawn.State;

namespace Banchou.Pawn {
    public class CombatantInstaller : MonoInstaller {
        public string PawnID { get; set; }

        public override void InstallBindings() {
            Container.Bind<string>().WithId("PawnID").FromInstance(PawnID);
            Container.Bind<PawnActions>().AsCached();

            Container.Bind<Rigidbody>().FromInstance(
                GetComponentInChildren<Rigidbody>()
            );
            Container.Bind<Animator>().FromInstance(
                GetComponentInChildren<Animator>()
            );
            Container.Bind<Part.GroundedVolume>().FromInstance(
                GetComponentInChildren<Part.GroundedVolume>()
            );
            Container.BindInterfacesAndSelfTo<Part.GroundMotor>().FromInstance(
                GetComponentInChildren<Part.GroundMotor>()
            );
            Container.BindInterfacesAndSelfTo<Part.LocalInputCommandStream>().FromInstance(
                GetComponentInChildren<Part.LocalInputCommandStream>()
            );
            Container.Bind<Part.LockOn>().FromInstance(
                GetComponentInChildren<Part.LockOn>()
            );
            Container.Bind<Part.Orientation>().FromInstance(
                GetComponentInChildren<Part.Orientation>()
            );
            Container.Bind<Part.Targettable>().FromInstance(
                GetComponentInChildren<Part.Targettable>()
            );
            Container.Bind<Part.ViewInputAxis>().FromInstance(
                GetComponentInChildren<Part.ViewInputAxis>()
            );
        }

        private void OnAnimatorMove() { }
    }
}