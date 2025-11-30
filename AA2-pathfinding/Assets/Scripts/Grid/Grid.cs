using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public class Grid : MonoBehaviour {

        [Header("Grid Settings")]
        public int width = 20;

        public int height = 20;
        public float cellSize = 1f;

        [Header("Prefabs")]
        [SerializeField] private GameObject nodePrefab;

        private Node[,] nodes;

        private void Awake() {
            if(nodePrefab == null) {
                Debug.LogError("⚠️ Node prefab missing!");
                return;
            }

            nodes = new Node[width, height];

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    Vector2Int gridPos = new(x, y);
                    Vector3 worldPos = GridToWorld(gridPos);

                    GameObject go = Instantiate(nodePrefab, worldPos, Quaternion.identity, transform);
                    go.transform.position = worldPos;

                    NodePrefab np = go.GetComponent<NodePrefab>();
                    if(np == null) {
                        Debug.LogError("Node prefab MUST contain NodePrefab component.");
                        continue;
                    }

                    nodes[x, y] = new Node(true, gridPos, worldPos, np.spriteRenderer);
                    nodes[x, y].ResetVisual();
                }
            }
        }

        public Vector3 GridToWorld(Vector2Int gridPos) {
            return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
        }

        public Vector2Int WorldToGrid(Vector3 worldPos) {
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x / cellSize),
                Mathf.RoundToInt(worldPos.y / cellSize)
            );
        }

        public Node GetNode(Vector2Int pos) {
            if(pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height)
                return null;

            return nodes[pos.x, pos.y];
        }

        public Node GetNodeFromWorld(Vector3 worldPos) => GetNode(WorldToGrid(worldPos));

        public int CalculateCost(Node from, Node to) {
            if(from.pos.x != to.pos.x && from.pos.y != to.pos.y)
                return 14;
            return 10;
        }

        public IEnumerable<Node> GetNeighbors(Node node, bool allowDiagonals = false) {
            Vector2Int[] dirs = {
                Vector2Int.up, Vector2Int.down,
                Vector2Int.left, Vector2Int.right
            };

            foreach(var d in dirs) {
                Node n = GetNode(node.pos + d);
                if(n != null && n.Walkable) yield return n;
            }

            if(allowDiagonals) {
                Vector2Int[] diag = {
                    new Vector2Int(1,1), new Vector2Int(1,-1),
                    new Vector2Int(-1,1), new Vector2Int(-1,-1)
                };
                foreach(var d in diag) {
                    Node n = GetNode(node.pos + d);
                    if(n != null && n.Walkable) yield return n;
                }
            }
        }

        public IEnumerable<Node> GetNodes() {
            foreach(Node node in nodes) yield return node;
        } 
    }
}