using UnityEngine;

namespace AI {

    public class DynamicObstacle : MonoBehaviour {
        public Grid grid;
        private Vector2Int lastPos;

        private void Update() {
            Vector2Int pos = Vector2Int.RoundToInt(transform.position);
            if(pos != lastPos) {
                grid.GetNode(lastPos).SetWalkable(true);
                grid.GetNode(pos).SetWalkable(false);
                lastPos = pos;
            }
        }
    }
}