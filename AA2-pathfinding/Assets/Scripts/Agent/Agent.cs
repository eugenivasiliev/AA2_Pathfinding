using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public class Agent : MonoBehaviour {
        [SerializeField] private Grid grid;
        private IAlgorithm algorithm;

        public void SetAlgorithm(IAlgorithm algo) {
            algorithm = algo;
        }

        public void MoveTo(Vector2Int target) {
            Node start = grid.GetNode(Vector2Int.RoundToInt(transform.position));
            Node goal = grid.GetNode(target);

            List<Node> path = algorithm.FindPath(grid, start, goal, out int explored);

            Debug.Log($"{algorithm.Name} explored {explored} nodes.");

            StopAllCoroutines();
            StartCoroutine(FollowPath(path));
        }

        private IEnumerator FollowPath(List<Node> path) {
            foreach(Node node in path) {
                Vector3 targetPos = new Vector3(node.GridPos.x, 0, node.GridPos.y);
                while(Vector3.Distance(transform.position, targetPos) > 0.1f) {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5);
                    yield return null;
                }
            }
        }
    }
}