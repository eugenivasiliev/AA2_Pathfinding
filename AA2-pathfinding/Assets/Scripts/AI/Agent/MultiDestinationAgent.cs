using System.Collections;
using System.Collections.Generic;
using AI.Interfaces;
using AI.UI;
using UnityEngine;

namespace AI {

    public class MultiDestinationAgent : MonoBehaviour {
        [SerializeField] private Grid grid;
        [SerializeField] public Agent agent;
        [SerializeField] private bool multiDestinationEnabled = true;
        [SerializeField] private bool bruteForceTSP = true;

        private Queue<Node> destinations = new Queue<Node>();
        private bool isMoving = false;
        private bool queueActive = false;

        public void OptimizeDestinations()
        {
            bruteForceTSP = !bruteForceTSP;
        }

        private void Awake()
        {
            agent = GetComponent<Agent>();
        }

        private void Update()
        {
            if (destinations.Count > 0)
                Debug.Log("Waiting destinations: " + destinations.Count);
        }

        public void AddDestination(Node node)
        {
            destinations.Enqueue(node);
        }
        public void StartMovement()
        {
            PathfindingVisualizer.Instance?.ResetAll(grid);
            if (isMoving || destinations.Count == 0)
                return;

            queueActive = true;
            OptimizeQueue();
            MoveToNextDestination();
        }

        private void MoveToNextDestination()
        {
            if (!queueActive || destinations.Count == 0)
            {
                isMoving = false;
                return;
            }

            isMoving = true;
            Node next = destinations.Dequeue();
            agent.MoveTo(next);
            StartCoroutine(WaitUntilArrival(next));
        }

        private IEnumerator WaitUntilArrival(Node target)
        {
            Vector3 finalPos = new Vector3(target.WorldPosition.x, target.WorldPosition.y, agent.transform.position.z);

            while (agent.transform.position != finalPos)
                yield return null;

            MoveToNextDestination();
        }
        public void ClearDestinations()
        {
            destinations.Clear();
            isMoving = false;
            queueActive = false;
        }

        public void OptimizeQueue()
        {
            if (destinations.Count <= 1) return;

            List<Node> nodes = new List<Node>(destinations);
            destinations.Clear();

            List<Node> optimizedOrder;
            Vector3 startPos = agent.transform.position;
            if (bruteForceTSP) {
                optimizedOrder = SolveTSPBruteForce(startPos, nodes);
            }
            else
            {
                optimizedOrder = SolveTSP(startPos, nodes);
            }

            foreach (var n in optimizedOrder)
                destinations.Enqueue(n);
        }

        private List<Node> SolveTSPBruteForce(Vector3 startPos, List<Node> nodes)
        {
            List<Node> best = null;
            float bestCost = float.MaxValue;

            foreach (var perm in Permute(nodes))
            {
                float cost = 0f;
                Vector3 currentPos = startPos;

                foreach (var node in perm)
                {
                    cost += Vector3.Distance(currentPos, node.WorldPosition);
                    currentPos = node.WorldPosition;
                }

                if (cost < bestCost)
                {
                    bestCost = cost;
                    best = new List<Node>(perm);
                }
            }

            return best;
        }

        private List<Node> SolveTSP(Vector3 startPos, List<Node> nodes)
        {
            List<Node> remaining = new List<Node>(nodes);
            List<Node> path = new List<Node>();
            Vector3 currentPos = startPos;

            while (remaining.Count > 0)
            {
                Node nearest = remaining[0];
                float minDist = Vector3.Distance(currentPos, nearest.WorldPosition);

                foreach (var n in remaining)
                {
                    float dist = Vector3.Distance(currentPos, n.WorldPosition);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = n;
                    }
                }

                path.Add(nearest);
                remaining.Remove(nearest);
                currentPos = nearest.WorldPosition;
            }

            return path;
        }

        private IEnumerable<List<Node>> Permute(List<Node> list)
        {
            int count = list.Count;
            int[] indexes = new int[count];

            for (int i = 0; i < count; i++)
                indexes[i] = 0;

            yield return new List<Node>(list);

            int k = 0;
            while (k < count)
            {
                if (indexes[k] < k)
                {
                    if (k % 2 == 0)
                    {
                        Swap(list, 0, k);
                    }
                    else
                    {
                        Swap(list, indexes[k], k);
                    }

                    yield return new List<Node>(list);

                    indexes[k]++;
                    k = 0;
                }
                else
                {
                    indexes[k] = 0;
                    k++;
                }
            }
        }
        private void Swap(List<Node> list, int a, int b)
        {
            Node temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
    }
}