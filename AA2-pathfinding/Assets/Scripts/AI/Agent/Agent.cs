using System;
using System.Collections;
using System.Collections.Generic;
using AI.Interfaces;
using AI.UI;
using UnityEngine;

namespace AI {

    public class Agent : MonoBehaviour {
        [SerializeField] private Grid grid;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private static Agent instance;
        public static Agent Instance { get { return instance; } }

        private IAlgorithm algorithm;
        private Coroutine followRoutine;
        public Coroutine FollowRoutine { get => followRoutine; }

        private List<Node> path;

        private void Start()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void SetAlgorithm(IAlgorithm algo) {
            algorithm = algo;
            ChangeColor(UnityEngine.Random.ColorHSV());
        }

        public void MoveTo(Node goal) {
            if(algorithm == null) {
                Debug.LogError("No algorithm assigned to Agent.");
                return;
            }

            Node start = grid.GetNodeFromWorld(transform.position);

            if(start == null || goal == null) {
                Debug.LogWarning("Start or goal node invalid.");
                return;
            }

            this.path = algorithm.FindPath(grid, start, goal, out List<Node> explored);
            PathfindingVisualizer.Instance?.ShowExplored(explored);

            if(path == null || path.Count == 0) {
                Debug.LogWarning("No path found.");
                return;
            }

            PathfindingVisualizer.Instance?.ShowPath(path);

            if(followRoutine != null)
                StopCoroutine(followRoutine);

            followRoutine = StartCoroutine(FollowPath(path));
        }

        private void ChangeColor(Color _newColor) {
            spriteRenderer.color = _newColor;
        }

        private IEnumerator FollowPath(List<Node> path) {
            foreach(Node node in path) {
                Vector3 target = new Vector3(
                    node.WorldPosition.x,
                    node.WorldPosition.y,
                    transform.position.z
                );

                while((transform.position - target).sqrMagnitude > 0.01f) {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target,
                        moveSpeed * Time.deltaTime
                    );
                    yield return null;
                }

                transform.position = target;
            }

            followRoutine = null;
        }

        public void UpdatePath()
        {
            if (followRoutine != null)
                StopCoroutine(followRoutine);

            if (algorithm is not IDynamic) return;

            path = (algorithm as IDynamic).UpdatePath(grid.GetNodeFromWorld(transform.position), out List<Node> exploredNodes);

            PathfindingVisualizer.Instance?.ShowExplored(exploredNodes);
            PathfindingVisualizer.Instance?.ShowPath(path);

            followRoutine = StartCoroutine(FollowPath(path));
        }
    }
}