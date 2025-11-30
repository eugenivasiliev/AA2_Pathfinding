using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AI.Input {

    public class MultiNodeInputHandler : MonoBehaviour {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Grid grid;
        public bool clearPath = false;
        public bool clearExplored = false;

        [Header("References")]
        [SerializeField] private MultiDestinationAgent multiAgent;
        [SerializeField] private Agent singleAgent;

        [Header("Mode")]
        [SerializeField] public bool multiDestinationMode = false;

        private NodeInputHandler nodeInput;

        public void multiDestination()
        {
            multiDestinationMode = !multiDestinationMode;
            Debug.Log("multiDestinationMode changed");
            Debug.Log(multiDestinationMode);
        }

        private void Awake()
        {
            if (worldCamera == null)
                worldCamera = Camera.main;
            nodeInput = GetComponent<NodeInputHandler>();
        }

        private void Update()
        {

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                HandleClick(true, false);
            }

            

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                clearPath = true;
                clearExplored = true;

                multiAgent.StartMovement();
            }

            if (multiDestinationMode == true)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    HandleClick(false, true);
                }
                nodeInput.enabled = false;
            }

            if (multiDestinationMode == false)
            {
                nodeInput.enabled = true;
            }
        }

        private void HandleClick(bool setGoal, bool toggleObstacle)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            worldPos.z = 0f;

            Node node = grid.GetNodeFromWorld(worldPos);
            if (node == null) return;

            if (setGoal && node.Walkable)
            {
                if (node == null || !node.Walkable)
                    return;

                if (multiDestinationMode)
                {
                    multiAgent.AddDestination(node);
                    node.UpdateDestinationVisual();
                }
                else
                {
                    singleAgent.MoveTo(node);
                }
            }

            if (toggleObstacle)
            {
                node.ToggleWalkable();
            }
        }

        public void SetMultiDestinationMode(bool enabled)
        {
            multiDestinationMode = enabled;
        }
    }
}