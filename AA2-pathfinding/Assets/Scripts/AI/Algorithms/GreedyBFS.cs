using System.Collections.Generic;
using AI.Interfaces;

namespace AI {

    public class GreedyBFS : IAlgorithm {
        public string Name => nameof(GreedyBFS);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            throw new System.NotImplementedException();
        }
    }
}