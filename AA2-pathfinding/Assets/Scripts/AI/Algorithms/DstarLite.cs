using System;
using System.Collections.Generic;
using System.Linq;
using AI.Interfaces;
using AI.Utils;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace AI
{
    public class DstarLite : IAlgorithm, IDynamic
    {
        public string Name => nameof(DstarLite);

        private PriorityQueue<Node, Key> U = new PriorityQueue<Node, Key>();
        private Dictionary<Node, float> rhs = new Dictionary<Node, float>();
        private Dictionary<Node, float> g = new Dictionary<Node, float>();

        private Grid grid;

        private Node s_last;
        private Node start;
        private Node goal;

        private float k_m = 0;

        public List<Node> FindPath(Grid grid, Node start, Node goal, out List<Node> exploredNodes)
        {
            List<Node> result = new List<Node>();
            this.s_last = start;
            Initialise(grid, start, goal);
            ComputeShortestPath();

            exploredNodes = g.Where(kvp => kvp.Value < float.PositiveInfinity)
                            .Select(kvp => kvp.Key)
                            .ToList();

            this.s_last = this.start;
            int maxSteps = 500;

            while (s_last != this.goal && maxSteps > 0)
            {
                maxSteps--;
                result.Add(s_last);

                Node bestNeighbor = null;
                float minCost = float.MaxValue;

                foreach (Node neighbor in grid.GetNeighbors(s_last))
                {
                    if (g.ContainsKey(neighbor) && g[neighbor] < float.PositiveInfinity)
                    {
                        float totalCost = grid.CalculateCost(s_last, neighbor) + g[neighbor];
                        if (totalCost < minCost && !result.Contains(neighbor))
                        {
                            minCost = totalCost;
                            bestNeighbor = neighbor;
                        }
                    }
                }

                if (bestNeighbor == null) break;

                s_last = bestNeighbor;
            }

            if (s_last == this.goal) result.Add(this.goal);

            return result;

            //List<Node> result = new List<Node>();
            //this.s_last = start;
            //Initialise(grid, start, goal);
            //ComputeShortestPath();

            //exploredNodes = new List<Node>();

            //int count = 500;

            //while (this.start != this.goal)
            //{
            //    --count;
            //    if(count == 0) return result;
            //    result.Add(this.start);

            //    float min = float.MaxValue;
            //    Node minNode = null;
            //    foreach (Node node in grid.GetNeighbors(this.start))
            //        if (min > grid.CalculateCost(this.start, node) + g[node] && !result.Contains(node)) minNode = node;

            //    this.start = minNode;
            //}
            //    float min = float.MaxValue;
            //    Node minNode = null;
            //    foreach (Node node in grid.GetNeighbors(this.start))
            //        if(min > grid.CalculateCost(this.start, node) + g[node]) minNode = node;

            //    k_m = k_m + Heuristic(this.s_last, this.start);
            //    this.s_last = this.start;
            //    /*Update edges*/
            //}
        }

        private void Initialise(Grid grid, Node start, Node goal)
        {
            this.grid = grid;
            this.U = new PriorityQueue<Node, Key>();
            this.k_m = 0;
            this.start = start;
            this.goal = goal;

            this.g = new Dictionary<Node, float>();
            this.rhs = new Dictionary<Node, float>();
            foreach(Node node in grid.GetNodes())
            {
                this.g.Add(node, float.PositiveInfinity);
                this.rhs.Add(node, float.PositiveInfinity);
            }
            this.rhs[this.goal] = 0;
            this.U.Enqueue(this.goal, new Key(Heuristic(this.start, this.goal), 0));
        }

        private Key ComputeKey(Node node)
        {
            if (!g.TryGetValue(node, out float g_n)) throw new System.Exception("Node not in g!");
            if (!rhs.TryGetValue(node, out float rhs_n)) throw new System.Exception("Node not in rhs!");

            return new Key(Mathf.Min(g_n, rhs_n) + Heuristic(start, node) + k_m, Mathf.Min(g_n, rhs_n));
        }

        private void UpdateNode(Node node)
        {
            if (g[node] != rhs[node] && U.Contains(node)) U.UpdatePriority(node, ComputeKey(node));
            else if (g[node] != rhs[node]) U.Enqueue(node, ComputeKey(node));
            else if (g[node] == rhs[node]) U.Remove(node);
        }

        private void SimpleEdgeUpdate(Node current)
        {
            k_m += Heuristic(s_last, current);
            s_last = current;

            bool costsChanged = false;

            foreach (Node neighbor in grid.GetNeighbors(current))
            {
                // Your logic to detect cost changes goes here
                // This could be based on:
                // - Dynamic obstacles appearing
                // - Terrain cost changes  
                // - Sensor input
                // - External events

                if (true)
                {
                    costsChanged = true;
                    break;
                }
            }

            if (costsChanged)
            {
                ComputeShortestPath();
            }
        }

        private void ComputeShortestPath()
        {
            while (U.TopKey().CompareTo(ComputeKey(start)) < 0 || rhs[start] > g[start])
            {
                Key k_old = U.TopKey();
                Node u = U.Dequeue();
                Key k_new = ComputeKey(u);
                if (k_old.CompareTo(k_new) < 0) U.UpdatePriority(u, k_new);
                else if (g[u] > rhs[u])
                {
                    g[u] = rhs[u];
                    U.Remove(u);
                    foreach (Node node in grid.GetNeighbors(u))
                    {
                        if (node != goal) rhs[node] = Mathf.Min(rhs[node], grid.CalculateCost(node, u) + g[u]);
                        UpdateNode(node);
                    }
                }
                else
                {
                    float g_old = g[u];
                    g[u] = float.PositiveInfinity;
                    foreach (Node node in grid.GetNeighbors(u).Union(new List<Node> { u }))
                    {
                        if (rhs[node] == grid.CalculateCost(node, u) + g_old && node != goal)
                        {
                            float min = float.MaxValue;
                            foreach (Node node_ in grid.GetNeighbors(u))
                                min = Mathf.Min(grid.CalculateCost(node, node_) + g[node_], min);
                        }
                        UpdateNode(node);

                    }
                }
            }
        }

        public class Key : IComparable
        {
            float k1;
            float k2;

            public Key(float _k1, float _k2) { k1 = _k1; k2 = _k2; }

            public int CompareTo(object obj)
            {
                if((obj as Key) == null) throw new System.NotImplementedException();
                Key other = (Key)obj;

                if (this == other) return 0;
                if (this <= other) return -1;
                return 1;
            }

            public override bool Equals(object obj)
            {
                return obj is Key key &&
                       k1 == key.k1 &&
                       k2 == key.k2;
            }

            public override int GetHashCode() => HashCode.Combine(k1, k2);
            public static bool operator ==(Key left, Key right) => left.Equals(right);
            public static bool operator !=(Key left, Key right) =>
                !(left.k1 == right.k1 && left.k2 == right.k2);
            public static bool operator <=(Key left, Key right) =>
                (left.k1 < right.k1) || (left.k1 == right.k1 && left.k2 <= right.k2);
            public static bool operator >=(Key left, Key right) =>
                (left.k1 > right.k1) || (left.k1 == right.k1 && left.k2 >= right.k2);
        }

        private int Heuristic(Node a, Node b)
        {
            return Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y);
        }

        public List<Node> UpdatePath(Node start, out List<Node> exploredNodes)
        {
            SimpleEdgeUpdate(this.s_last);

            List<Node> result = new List<Node>();

            exploredNodes = g.Where(kvp => kvp.Value < float.PositiveInfinity)
                            .Select(kvp => kvp.Key)
                            .ToList();

            this.s_last = start;
            int maxSteps = 500;

            while (s_last != this.goal && maxSteps > 0)
            {
                maxSteps--;
                result.Add(s_last);

                Node bestNeighbor = null;
                float minCost = float.MaxValue;

                foreach (Node neighbor in grid.GetNeighbors(s_last))
                {
                    if (g.ContainsKey(neighbor) && g[neighbor] < float.PositiveInfinity)
                    {
                        float totalCost = grid.CalculateCost(s_last, neighbor) + g[neighbor];
                        if (totalCost < minCost && !result.Contains(neighbor))
                        {
                            minCost = totalCost;
                            bestNeighbor = neighbor;
                        }
                    }
                }

                if (bestNeighbor == null) break;

                s_last = bestNeighbor;
            }

            if (s_last == this.goal) result.Add(this.goal);

            return result;
        }
    }
}