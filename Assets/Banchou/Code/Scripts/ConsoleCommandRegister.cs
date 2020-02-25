using System;
using System.Linq;
using IngameDebugConsole;
using Zenject;
using UnityEngine;

using Banchou.Board;
using Banchou.Pawn;
using Banchou.Combatant;

namespace Banchou.Debug {
    public class ConsoleCommandRegister {
        private Vector3 GetMousePosition() {
            RaycastHit hitInfo;
            var ray = Camera.main.ViewportPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hitInfo);
            return hitInfo.point;
        }

        [Inject]
        public void RegisterBoardActions(
            Dispatcher dispatch,
            GetState getState,
            BoardActions actions
        ) {
            DebugLogConsole.AddCommand(
                command: "AddPawn",
                description: "Adds a new Pawn to the Board",
                method: (string prefabKey, string displayName, float cameraWeight) => {
                    dispatch(actions.AddPawn(prefabKey, displayName, cameraWeight, GetMousePosition()));
                }
            );

            DebugLogConsole.AddCommand(
                command: "AddCombatant",
                description: "Adds a Combatant Pawn to the Board",
                method: (string prefabKey, int health, string displayName, float cameraWeight) => {
                    dispatch(actions.AddCombatant(prefabKey, health, displayName, cameraWeight, GetMousePosition()));
                }
            );

            DebugLogConsole.AddCommand(
                command: "RemovePawn",
                description: "Removes a pawn from the board",
                method: (string id) => { dispatch(actions.RemovePawn(id)); }
            );
        }

        [Inject]
        public void RegisterPawnActions(
            Dispatcher dispatch,
            CombatantActions actions
        ) {
            DebugLogConsole.AddCommand(
                command: "Hurt",
                description: "Apply damage to a Combatant",
                method: (string id, int amount, Vector3 push) => {
                    dispatch(actions.Hurt(pawnID: id, amount: amount, push: push));
                }
            );

            DebugLogConsole.AddCommand(
                command: "Heal",
                description: "Heal a Combatant",
                method: (string id, int amount) => {
                    dispatch(actions.Heal(pawnID: id, amount: amount));
                }
            );

            DebugLogConsole.AddCommand(
                command: "Launch",
                description: "Launch a Combatant",
                method: (string id, Vector3 push) => {
                    dispatch(actions.Launch(id));
                }
            );
        }
    }
}