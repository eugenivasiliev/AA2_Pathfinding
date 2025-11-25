using System.Collections.Generic;
using AI.Interfaces;

namespace AI {

    public class Astar : IAlgorithm {
        public string Name => nameof(Astar);

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes) {
            throw new System.NotImplementedException();
        }
    }
}