using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public interface IDynamic
    {
        List<Node> UpdatePath(Node start, out List<Node> exploredNodes);
    }
}