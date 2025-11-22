using UnityEngine;
using System.Collections.Generic;

public class AgentController : MonoBehaviour
{
    public GridManager graph;
    public AgentMover mover;

    private IPathfindingAlgorithm pathfinder;
    private List<GridNode> currentPath;

    public void SetAlgorithm(IPathfindingAlgorithm algorithm)
    {
        pathfinder = algorithm;
        TryRepath();
    }

    public void TryRepath()
    {
        if (pathfinder == null) return;

        Vector2Int startPos = Vector2Int.FloorToInt(transform.position);
        Vector2Int targetPos = mover.Target;

        GridNode start = graph.GetNode(startPos);
        GridNode goal = graph.GetNode(targetPos);

        currentPath = pathfinder.FindPath(start, goal, graph);
        mover.Follow(currentPath);
    }
}