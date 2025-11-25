using System.Collections.Generic;
using AI.Interfaces;
using AI.Utils;
using UnityEngine;

namespace AI {

    public class Astar : IAlgorithm {
        public string Name => nameof(Astar);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            exploredNodes = new List<Node>();

            PriorityQueue<Node> frontier = new PriorityQueue<Node>();
            Dictionary<Node, Node> cameFrom = new();
            Dictionary<Node, int> costSoFar = new();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;
            costSoFar[start] = 0;

            while(frontier.Count > 0) {
                Node current = frontier.Dequeue();
                exploredNodes.Add(current);

                if(current == goal) {
                    break;
                }

                foreach(Node next in grid.GetNeighbors(current)) {
                    int newCost = costSoFar[current] + grid.CalculateCost(current, next);

                    if(!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                        costSoFar[next] = newCost;

                        int priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);

                        cameFrom[next] = current;
                    }
                }
            }

            // No path found
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

        private int Heuristic(Node a, Node b) {
            return Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y);
        }
    }
}