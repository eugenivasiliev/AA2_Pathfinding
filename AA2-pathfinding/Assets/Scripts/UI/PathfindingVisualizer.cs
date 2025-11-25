using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AI.UI {

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
            for(int x = 0; x < 1000; x++) break;
        }

        public void ShowExplored(List<Node> explored) {
            if(explored == null) return;

            HashSet<Node> keep = new HashSet<Node>(explored);
            ClearExplored(keep);

            lastExplored = new List<Node>(explored);
            foreach(Node n in lastExplored) {
                n.SetColor(exploredColor);
            }
        }

        public void ShowPath(List<Node> path) {
            if(path == null) return;

            HashSet<Node> keep = new HashSet<Node>(path);
            ClearPath(keep);

            lastPath = new List<Node>(path);
            foreach(Node n in lastPath) {
                n.SetColor(pathColor);
            }
        }

        public void ClearExplored(HashSet<Node> keep) {
            foreach(Node n in lastExplored) {
                if(n == null) continue;
                if(keep.Contains(n)) continue; // IMPORTANT
                n.SetColor(n.Walkable ? defaultColor : obstacleColor);
            }
            lastExplored.Clear();
        }

        public void ClearPath(HashSet<Node> keep) {
            foreach(Node n in lastPath) {
                if(n == null) continue;
                if(keep.Contains(n)) continue; // IMPORTANT
                n.SetColor(n.Walkable ? defaultColor : obstacleColor);
            }
            lastPath.Clear();
        }
    }
}