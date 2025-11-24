using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public class Grid : MonoBehaviour {

        [Header("Grid settings")]
        [SerializeField] private int width = 20;

        [SerializeField] private int height = 20;
        [SerializeField] private float cellSize = 1f;

        [Header("Node prefab")]
        [SerializeField] private GameObject nodePrefab;

        private Node[,] nodes;

        private void Awake() {
            nodes = new Node[width, height];

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Vector2Int pos = new(x, y);
                    GameObject instance = Instantiate(nodePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity, transform);
                    nodes[x, y] = new Node(true, pos, instance.GetComponent<NodePrefab>().spriteRenderer);
                }
            }
        }

        public Node GetNode(Vector2Int pos) {
            if(pos.x < 0 || pos.x >= width ||
                pos.y < 0 || pos.y >= height)
                return null;

            return nodes[pos.x, pos.y];
        }

        public List<Node> GetNeighbors(Node node) {
            List<Node> neighbors = new();

            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach(var d in dirs) {
                var n = GetNode(node.GridPos + d);
                if(n != null && n.Walkable)
                    neighbors.Add(n);
            }

            return neighbors;
        }
    }
}