using System.Collections;
using System.Collections.Generic;
using AI;
using AI.Algorithms;
using AI.Interfaces;
using AI.UI;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Statistics
{
    public class Statistics
    {
        private static Statistics instance;
        public static Statistics Instance { get { return instance; } set { instance = value; } }

        public readonly int TestCount = 20;

        public readonly int MinGridSize = 20;
        public readonly int MaxGridSize = 50;

        public readonly float WallProbability = 0.3f;

        public bool isActive = false;

        public AI.Grid grid;

        public VerticalLayoutGroup layoutGroup;
        public GameObject sliderPrefab;
        public GameObject textPrefab;
        
        public List<IAlgorithm> algorithms = new List<IAlgorithm>() { new BFS(), new GreedyBFS(), new Dijkstra(), new Astar() };
        public Dictionary<string, int> exploredNodesCount = new Dictionary<string, int>();
        public Dictionary<string, int> minExploredNodesCount = new Dictionary<string, int>();
        public Dictionary<string, int> maxExploredNodesCount = new Dictionary<string, int>();
        public Dictionary<string, int> pathLengthsCount = new Dictionary<string, int>();
        public Dictionary<string, int> minPathLengthsCount = new Dictionary<string, int>();
        public Dictionary<string, int> maxPathLengthsCount = new Dictionary<string, int>();

        public void Init()
        {
            exploredNodesCount.Clear();
            minExploredNodesCount.Clear();
            maxExploredNodesCount.Clear();
            pathLengthsCount.Clear();
            minPathLengthsCount.Clear();
            maxPathLengthsCount.Clear();
            
            foreach (var algorithm in algorithms)
            {
                exploredNodesCount[algorithm.Name] = 0;
                minExploredNodesCount[algorithm.Name] = int.MaxValue;
                maxExploredNodesCount[algorithm.Name] = 0;
                pathLengthsCount[algorithm.Name] = 0;
                minPathLengthsCount[algorithm.Name] = int.MaxValue;
                maxPathLengthsCount[algorithm.Name] = 0;
            }
        }

        public void DisplayStats()
        {
            int maxPathCount = 0;
            int maxExploredCount = 0;
            foreach (var algorithm in algorithms)
            {
                maxPathCount = Mathf.Max(maxPathCount, pathLengthsCount[algorithm.Name]);
                maxExploredCount = Mathf.Max(maxExploredCount, exploredNodesCount[algorithm.Name]);
            }

            Debug.Log(maxPathCount + " " +  maxExploredCount);

            foreach (GameObject child in layoutGroup.transform)
            {
                GameObject.Destroy(child);
            }

            GameObject subVert = new GameObject();
            subVert.AddComponent<VerticalLayoutGroup>();
            subVert.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
            
            GameObject pathLengths = GameObject.Instantiate(subVert, layoutGroup.transform);
            GameObject labelInstance = GameObject.Instantiate(textPrefab, pathLengths.transform);
            labelInstance.GetComponent<TMP_Text>().text = "Paths: ";
            GameObject exploredNodes = GameObject.Instantiate(subVert, layoutGroup.transform);
            labelInstance = GameObject.Instantiate(textPrefab, exploredNodes.transform);
            labelInstance.GetComponent<TMP_Text>().text = "Explored Nodes: ";

            foreach (var algorithm in algorithms)
            {
                labelInstance = GameObject.Instantiate(textPrefab, pathLengths.transform);
                labelInstance.GetComponent<TMP_Text>().text = 
                    algorithm.Name + 
                    ": Min " + minPathLengthsCount[algorithm.Name] + 
                    " Max " + maxPathLengthsCount[algorithm.Name] + 
                    " Avg " + (float)pathLengthsCount[algorithm.Name] / (float)TestCount;
                GameObject instance = GameObject.Instantiate(sliderPrefab, pathLengths.transform);
                instance.name = algorithm.Name;
                instance.GetComponent<Slider>().value = ((float)pathLengthsCount[algorithm.Name]) / ((float)maxPathCount);

                labelInstance = GameObject.Instantiate(textPrefab, exploredNodes.transform);
                labelInstance.GetComponent<TMP_Text>().text =
                    algorithm.Name +
                    ": Min " + minExploredNodesCount[algorithm.Name] +
                    " Max " + maxExploredNodesCount[algorithm.Name] +
                    " Avg " + (float)exploredNodesCount[algorithm.Name] / (float)TestCount;
                instance = GameObject.Instantiate(sliderPrefab, exploredNodes.transform);
                instance.name = algorithm.Name;
                instance.GetComponent<Slider>().value = ((float)exploredNodesCount[algorithm.Name]) / ((float)maxExploredCount);
            }

        }

        public IEnumerator PerformTest()
        {

            Init();

            isActive = true;

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

                UnityEngine.Vector2Int startPos;
                Node start;

                do
                {
                    startPos = new UnityEngine.Vector2Int(
                        UnityEngine.Random.Range(0, grid.width), UnityEngine.Random.Range(0, grid.height)
                        );
                    start = grid.GetNode(startPos);
                } while (!start.Walkable);

                UnityEngine.Vector2Int goalPos;
                Node goal;

                do
                {
                    goalPos = new UnityEngine.Vector2Int(
                        UnityEngine.Random.Range(0, grid.width), UnityEngine.Random.Range(0, grid.height)
                        );
                    goal = grid.GetNode(goalPos);
                } while(!goal.Walkable);

                foreach (IAlgorithm algorithm in algorithms)
                {
                    PathfindingVisualizer.Instance.ResetAll(grid);

                    Agent.Instance.SetAlgorithm(algorithm);
                    Agent.Instance.transform.position = start.WorldPosition;
                    Agent.Instance.MoveTo(goal);

                    while(Agent.Instance.FollowRoutine != null) yield return new WaitForEndOfFrame();
                }

                tests++;
            }

            DisplayStats();

            isActive = false;

            yield return null;
        }
    }
}
