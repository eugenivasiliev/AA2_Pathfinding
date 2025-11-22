using System.Collections.Generic;
using UnityEngine;

public interface IGraph<TNode>
{
    TNode GetNode(Vector2Int pos);

    IEnumerable<TNode> GetNeighbors(TNode node);

    void ResetNodes();
}