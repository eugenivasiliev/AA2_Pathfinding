using System.Collections.Generic;

public interface IPathfindingAlgorithm
{
    string Name { get; }
    int ExploredNodes { get; }

    List<GridNode> FindPath(GridNode start, GridNode goal, IGraph<GridNode> graph);
}