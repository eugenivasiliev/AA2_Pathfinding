using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour, IGraph<GridNode>
{
    private GridNode[,] grid;

    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    public LayerMask obstacleMask;

    private void Awake()
    {
        BuildGrid();
    }

    private void BuildGrid()
    {
        grid = new GridNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool walkable = !Physics2D.OverlapPoint(new Vector2(x, y), obstacleMask);
                grid[x, y] = new GridNode(new Vector2Int(x, y), walkable);
            }
        }
    }

    public GridNode GetNode(Vector2Int pos) => grid[pos.x, pos.y];

    public IEnumerable<GridNode> GetNeighbors(GridNode node)
    {
        Vector2Int[] dirs =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var d in dirs)
        {
            int nx = node.Position.x + d.x;
            int ny = node.Position.y + d.y;

            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
            {
                var neigh = grid[nx, ny];
                if (neigh.Walkable) yield return neigh;
            }
        }
    }

    public void ResetNodes()
    {
        foreach (var node in grid)
        {
            node.Reset();
        }
    }
}