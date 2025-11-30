using System.Collections.Generic;
using UnityEngine;

namespace AI.Interfaces {

    public interface IAlgorithm {
        string Name { get; }

        List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes);

        void UploadData(List<Node> path, List<Node> explored)
        {
            if(!Statistics.Statistics.Instance.isActive) return;

            Statistics.Statistics.Instance.pathLengthsCount[this.Name] += path.Count;
            Statistics.Statistics.Instance.minPathLengthsCount[this.Name] = 
                Mathf.Min(Statistics.Statistics.Instance.minPathLengthsCount[this.Name], path.Count);
            Statistics.Statistics.Instance.maxPathLengthsCount[this.Name] =
                Mathf.Max(Statistics.Statistics.Instance.maxPathLengthsCount[this.Name], path.Count);

            Statistics.Statistics.Instance.exploredNodesCount[this.Name] += explored.Count;
            Statistics.Statistics.Instance.minExploredNodesCount[this.Name] =
                Mathf.Min(Statistics.Statistics.Instance.minExploredNodesCount[this.Name], explored.Count);
            Statistics.Statistics.Instance.maxExploredNodesCount[this.Name] =
                Mathf.Max(Statistics.Statistics.Instance.maxExploredNodesCount[this.Name], explored.Count);
        }
    }
}