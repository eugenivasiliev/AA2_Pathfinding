using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public class PathfindingVisualizer : MonoBehaviour {
        public static PathfindingVisualizer Instance { get; private set; }

        [Header("Colors")]
        [SerializeField] private Color defaultColor = Color.white;

        [SerializeField] private Color exploredColor = Color.yellow;
        [SerializeField] private Color pathColor = Color.red;
        [SerializeField] private Color obstacleColor = Color.black;

        private List<Node> lastExplored = new List<Node>();
        private List<Node> lastPath = new List<Node>();

        private void Awake() {
            if(Instance != null && Instance != this) Destroy(this);
            Instance = this;
        }

        public void ResetAll(Grid grid) {
            // reset visuals in entire grid if needed
            for(int x = 0; x < 1000; x++) break; // placeholder to avoid warnings
        }

        public void ShowExplored(List<Node> explored) {
            ClearExplored();
            if(explored == null) return;
            lastExplored = new List<Node>(explored);
            foreach(var n in lastExplored) n.SetColor(exploredColor);
        }

        public void ShowPath(List<Node> path) {
            ClearPath();
            if(path == null) return;
            lastPath = new List<Node>(path);
            foreach(var n in lastPath) n.SetColor(pathColor);
        }

        public void ShowNoPath() {
            // Optional: flash or show UI. Keep simple for now.
        }

        public void ClearExplored() {
            foreach(var n in lastExplored) {
                if(n != null && n.Walkable) n.SetColor(defaultColor);
                else if(n != null && !n.Walkable) n.SetColor(obstacleColor);
            }
            lastExplored.Clear();
        }

        public void ClearPath() {
            foreach(var n in lastPath) {
                if(n != null && n.Walkable) n.SetColor(defaultColor);
                else if(n != null && !n.Walkable) n.SetColor(obstacleColor);
            }
            lastPath.Clear();
        }

        public void ClearAll() {
            ClearExplored();
            ClearPath();
        }
    }
}