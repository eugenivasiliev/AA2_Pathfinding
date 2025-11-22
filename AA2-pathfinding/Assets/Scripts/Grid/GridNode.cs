using UnityEngine;

public class GridNode
{
    private float G;
    private float H;
    private GridNode Parent;
    public Vector2Int Position { get; private set; }
    public bool Walkable { get; private set; }
    public float F => G + H;

    public GridNode(Vector2Int pos, bool walkable)
    {
        Position = pos;
        Walkable = walkable;
    }

    public void Reset()
    {
        G = H = 0;
        Parent = null;
    }
}