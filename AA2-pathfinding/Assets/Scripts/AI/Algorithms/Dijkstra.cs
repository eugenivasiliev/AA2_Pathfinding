using System.Collections.Generic;
using AI.Interfaces;
using AI.Utils;

namespace AI {

    public class Dijkstra : IAlgorithm {
        public string Name => nameof(Dijkstra);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            exploredNodes = new List<Node>();

            PriorityQueue<Node, float> frontier = new PriorityQueue<Node, float>();
            Dictionary<Node, Node> cameFrom = new();
            Dictionary<Node, int> costSoFar = new();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while(frontier.Count > 0) {
                Node current = frontier.Dequeue();
                exploredNodes.Add(current);

                if(current == goal)
                    break;

                foreach(Node neighbor in grid.GetNeighbors(current)) {
                    int movementCost = grid.CalculateCost(current, neighbor);
                    int newCost = costSoFar[current] + movementCost;

                    if(!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]) {
                        costSoFar[neighbor] = newCost;
                        frontier.Enqueue(neighbor, newCost);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            // If we never reached the goal
            if(!cameFrom.ContainsKey(goal)) {
                return null;
            }

            // Reconstruct path
            List<Node> path = new();
            for(Node n = goal; n != null; n = cameFrom[n]) {
                path.Add(n);
            }
            path.Reverse();

            return path;
        }
    }
}