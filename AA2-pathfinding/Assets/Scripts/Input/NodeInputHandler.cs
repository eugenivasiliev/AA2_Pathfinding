using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AI.Input {

    public class NodeInputHandler : MonoBehaviour {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Agent agent;
        [SerializeField] private Grid grid;
        [SerializeField] private VerticalLayoutGroup statsLayout;
        [SerializeField] private GameObject slider;
        [SerializeField] private GameObject text;

        private void Awake() {
            if(worldCamera == null) worldCamera = Camera.main;

            Statistics.Statistics.Instance = new Statistics.Statistics();

            Statistics.Statistics.Instance.grid = grid;
            Statistics.Statistics.Instance.layoutGroup = statsLayout;
            Statistics.Statistics.Instance.sliderPrefab = slider;
            Statistics.Statistics.Instance.textPrefab = text;

            InputSystem.actions.FindAction("Previous").started += ctx => {
                StartCoroutine(Statistics.Statistics.Instance.PerformTest());
                };

            InputSystem.actions.FindAction("Next").started += ctx =>
            {
                grid.width = 20;
                grid.height = 20;
                grid.cellSize = 1;

                grid.Init();

                agent.transform.position = Vector3.one * 10;
            };
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