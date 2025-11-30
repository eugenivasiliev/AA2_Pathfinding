using UnityEngine;
using UnityEngine.InputSystem;

namespace AI.Input {

    public class NodeInputHandler : MonoBehaviour {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Agent agent;
        [SerializeField] private Grid grid;

        private void Awake() {
            if(worldCamera == null) worldCamera = Camera.main;
        }

        private void Update() {
            if(Mouse.current.leftButton.wasPressedThisFrame) {
                HandleClick(true, false);
            }

            if(Mouse.current.rightButton.wasPressedThisFrame) {
                HandleClick(false, true);
            }
        }

        private void HandleClick(bool setGoal, bool toggleObstacle) {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            worldPos.z = 0f;

            Node node = grid.GetNodeFromWorld(worldPos);
            if(node == null) return;

            if(setGoal && node.Walkable) {
                agent.MoveTo(node);
            }

            if(toggleObstacle) {
                node.ToggleWalkable();
                agent.UpdatePath();
            }
        }
    }
}