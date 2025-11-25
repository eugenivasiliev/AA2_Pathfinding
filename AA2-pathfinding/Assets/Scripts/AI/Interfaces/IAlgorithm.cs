using System.Collections.Generic;

namespace AI.Interfaces {

    public interface IAlgorithm {
        string Name { get; }

        List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes);
    }
}