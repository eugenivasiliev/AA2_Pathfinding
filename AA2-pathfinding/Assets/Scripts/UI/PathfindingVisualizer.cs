using AI.Input;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AI.UI {

    public class PathfindingVisualizer : MonoBehaviour {
        public static PathfindingVisualizer Instance { get; private set; }
        [SerializeField] private MultiNodeInputHandler multiNodeInputHandler;

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
            foreach (Node node in grid.GetNodes())
            {
                if (node.Walkable)
                {
                    node.SetColor(defaultColor);
                } else
                {
                    node.SetColor(Color.black);
                }
            }
        }

        public void ShowExplored(List<Node> explored) {
            if(explored == null) return;

            HashSet<Node> keep = new HashSet<Node>(explored);
            ClearExplored(keep);

            lastExplored = new List<Node>(explored);
            foreach(Node n in lastExplored) {
                if(n.Path == false)
                    n.SetColor(exploredColor);
                n.SetAsPath(false);
            }
        }

        public void ShowPath(List<Node> path) {
            if(path == null) return;

            HashSet<Node> keep = new HashSet<Node>(path);
            ClearPath(keep);

            lastPath = new List<Node>(path);
            foreach(Node n in lastPath) {
                n.SetColor(pathColor);
                n.SetAsPath(true);
            }
        }

        public void ClearExplored(HashSet<Node> keep) {
            if (multiNodeInputHandler.multiDestinationMode == true && multiNodeInputHandler.clearExplored == false) return;
            foreach (Node n in lastExplored) {
                if(n == null) continue;
                if(keep.Contains(n)) continue; // IMPORTANT
                n.SetColor(n.Walkable ? defaultColor : obstacleColor);
            }
            lastExplored.Clear();
            multiNodeInputHandler.clearExplored = false;
        }

        public void ClearPath(HashSet<Node> keep) {
            if (multiNodeInputHandler.multiDestinationMode == true && multiNodeInputHandler.clearPath == false) return;
            foreach (Node n in lastPath) {
                if(n == null) continue;
                if(keep.Contains(n)) continue; // IMPORTANT
                
                n.SetAsPath(false);
                n.SetColor(n.Walkable ? defaultColor : obstacleColor);
            }
            lastPath.Clear();
            multiNodeInputHandler.clearPath = false;
        }
    }
}