using System.Collections.Generic;

namespace AI {

    public interface IAlgorithm {
        string Name { get; }

        List<Node> FindPath(Grid grid, Node start, Node goal, out int exploredNodes);
    }
}