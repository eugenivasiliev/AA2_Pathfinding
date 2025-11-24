using System.Collections.Generic;
using UnityEngine;

namespace AI {

    public class BFS : IAlgorithm {
        public string Name => nameof(BFS);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out int exploredNodes) {
            Queue<Node> queue = new();
            HashSet<Node> visited = new();
            Dictionary<Node, Node> cameFrom = new();

            exploredNodes = 0;

            queue.Enqueue(start);
            visited.Add(start);
            cameFrom[start] = null;

            while(queue.Count > 0) {
                Node current = queue.Dequeue();
                exploredNodes++;

                if(current == goal)
                    break;

                foreach(Node neighbour in grid.GetNeighbors(current)) {
                    if(!visited.Contains(neighbour)) {
                        visited.Add(neighbour);
                        queue.Enqueue(neighbour);

                        cameFrom[neighbour] = current;
                    }
                }
            }

            // If BFS could not reach the goal
            if(!cameFrom.ContainsKey(goal)) {
                return null;
            }

            // --- Reconstruct path ---
            List<Node> path = new();
            Node node = goal;

            while(node != null) {
                path.Insert(0, node);
                node.SetColorOfSprite(Color.red);
                node = cameFrom[node];
            }

            return path;
        }
    }
}