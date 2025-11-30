using System;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using static UnityEditor.Progress;

namespace AI.Utils {

    public class PriorityQueue<T1, T2> where T2 : IComparable
    {
        private readonly List<(T1 item, T2 priority)> items = new();

        public int Count => items.Count;

        public void Enqueue(T1 item, T2 priority) {
            items.Add((item, priority));
        }

        public T1 Dequeue() {
            int bestIndex = 0;

            for(int i = 1; i < items.Count; i++) {
                if(items[i].priority.CompareTo(items[bestIndex].priority) <= 0) {
                    bestIndex = i;
                }
            }

            T1 bestItem = items[bestIndex].item;
            items.RemoveAt(bestIndex);
            return bestItem;
        }

        public void Remove(T1 item)
        {
            for(int i = 0; i < items.Count; ++i)
                if (item.Equals(items[i].item))
                {
                    items.RemoveAt(i);
                    return;
                }
        }

        public bool Contains(T1 item)
        {
            foreach((T1 item, T2 priority) pair in items)
                if(item.Equals(pair.item)) return true;
            return false;
        }

        public void UpdatePriority(T1 value, T2 priority)
        {
            for (int i = 0; i < items.Count; ++i)
                if (value.Equals(items[i].Item1)) items[i] = (value, priority);
        }

        public T2 TopKey()
        {
            int bestIndex = 0;

            for (int i = 1; i < items.Count; i++)
            {
                if (items[i].priority.CompareTo(items[bestIndex].priority) <= 0)
                {
                    bestIndex = i;
                }
            }

            Debug.Assert(items.Count > 0);

            return items[bestIndex].priority;
        }
    }
}