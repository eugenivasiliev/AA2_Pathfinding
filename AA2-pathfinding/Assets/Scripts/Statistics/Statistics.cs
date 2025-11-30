using System.Collections;
using System.Collections.Generic;
using AI;
using AI.Algorithms;
using AI.Interfaces;
using UnityEngine;

namespace Statistics
{
    public class Statistics
    {
        private static Statistics instance;
        public static Statistics Instance { get { return instance; } set { instance = value; } }

        public readonly int TestCount = 20;

        public readonly int MinGridSize = 10;
        public readonly int MaxGridSize = 50;

        public readonly float WallProbability = 0.1f;

        public AI.Grid grid;
        
        private List<IAlgorithm> algorithms = new List<IAlgorithm>() { new BFS(), new GreedyBFS(), new Dijkstra(), new Astar(), new DstarLite() };
        private Dictionary<IAlgorithm, int> exploredNodesCount = new Dictionary<IAlgorithm, int>();
        private Dictionary<IAlgorithm, int> pathLengthsCount = new Dictionary<IAlgorithm, int>();

        public void Init()
        {
            exploredNodesCount.Clear();
            pathLengthsCount.Clear();
            foreach (var algorithm in algorithms)
            {
                exploredNodesCount[algorithm] = 0;
                pathLengthsCount[algorithm] = 0;
            }
        }

        public IEnumerator PerformTest()
        {
            Debug.Log("AAAAA");

            Init();

            int tests = 0;
            while (tests < TestCount)
            {
                grid.width = UnityEngine.Random.Range(MinGridSize, MaxGridSize);
                grid.height = UnityEngine.Random.Range(MinGridSize, MaxGridSize);
                grid.cellSize = 20f / Mathf.Max(grid.width, grid.height);

                grid.Init();

                foreach(Node node in grid.GetNodes())
                {
                    node.SetWalkable(UnityEngine.Random.Range(0f, 1f) >= WallProbability);
                }

                UnityEngine.Vector3 startPos = Agent.Instance.transform.position;
                Node start = grid.GetNodeFromWorld(startPos);

                UnityEngine.Vector2Int goalPos = new UnityEngine.Vector2Int(
                    UnityEngine.Random.Range(0, grid.width), UnityEngine.Random.Range(0, grid.height)
                    );

                Node goal = grid.GetNode(goalPos);

                foreach (IAlgorithm algorithm in algorithms)
                {
                    Agent.Instance.SetAlgorithm(algorithm);
                    Agent.Instance.transform.position = startPos;
                    Agent.Instance.MoveTo(goal);

                    while(Agent.Instance.FollowRoutine != null) yield return new WaitForEndOfFrame();
                }

                tests++;
            }

            yield return null;
        }
    }
}
