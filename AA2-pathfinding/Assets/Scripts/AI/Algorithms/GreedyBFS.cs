using System.Collections.Generic;
using AI.Interfaces;
using AI.Utils;
using UnityEngine;

namespace AI {

    public class GreedyBFS : IAlgorithm {
        public string Name => nameof(GreedyBFS);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            exploredNodes = new List<Node>();

            PriorityQueue<Node> frontier = new PriorityQueue<Node>();
            Dictionary<Node, Node> cameFrom = new();

            frontier.Enqueue(start, 0);
            cameFrom[start] = null;

            while(frontier.Count > 0) {
                Node current = frontier.Dequeue();
                exploredNodes.Add(current);

                if(current == goal)
                    break;

                foreach(Node neighbour in grid.GetNeighbors(current)) {
                    if(!cameFrom.ContainsKey(neighbour)) {
                        int priority = Heuristic(neighbour, goal);
                        frontier.Enqueue(neighbour, priority);
                        cameFrom[neighbour] = current;
                    }
                }
            }

            if(!cameFrom.ContainsKey(goal))
                return null;

            List<Node> path = new();
            for(Node n = goal; n != null; n = cameFrom[n])
                path.Add(n);

            path.Reverse();
            return path;
        }

        private int Heuristic(Node a, Node b) {
            return Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y);
        }
    }
}