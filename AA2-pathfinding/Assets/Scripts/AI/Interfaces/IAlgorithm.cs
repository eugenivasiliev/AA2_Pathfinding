using System.Collections.Generic;

namespace AI.Interfaces {

    public interface IAlgorithm {
        string Name { get; }

        List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes);

        void UploadData(List<Node> path, List<Node> explored)
        {
            if(!Statistics.Statistics.Instance.isActive) return;

            Statistics.Statistics.Instance.pathLengthsCount[this.Name] += path.Count;
            Statistics.Statistics.Instance.exploredNodesCount[this.Name] += explored.Count;
        }
    }
}