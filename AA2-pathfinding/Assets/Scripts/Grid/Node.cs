using UnityEngine;

namespace AI {

    public class Node {
        public bool Walkable { get; private set; }
        public Vector2Int GridPos { get; private set; }

        private Node Parent;
        private float GCost;
        private float HCost;
        public float FCost => GCost + HCost;

        public Node(bool walkable, Vector2Int pos) {
            Walkable = walkable;
            GridPos = pos;
        }
    }
}