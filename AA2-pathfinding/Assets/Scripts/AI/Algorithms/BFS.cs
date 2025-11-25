using System.Collections.Generic;
using AI.Interfaces;

namespace AI.Algorithms {

    public class BFS : IAlgorithm {
        public string Name => nameof(BFS);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            exploredNodes = new List<Node>();

            if(start == null || goal == null) return null;

            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            queue.Enqueue(start);
            visited.Add(start);
            cameFrom[start] = null;

            bool found = false;

            while(queue.Count > 0) {
                Node current = queue.Dequeue();
                exploredNodes.Add(current);

                if(current == goal) { found = true; break; }

                foreach(Node neighbor in grid.GetNeighbors(current)) {
                    if(visited.Add(neighbor)) {
                        queue.Enqueue(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            if(!found) return null;

            List<Node> path = new List<Node>();
            for(Node n = goal; n != null; n = cameFrom[n]) path.Add(n);
            path.Reverse();

            return path;
        }
    }
}