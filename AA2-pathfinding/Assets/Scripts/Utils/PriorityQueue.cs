using System.Collections.Generic;

namespace AI.Utils {

    public class PriorityQueue<T> {
        private readonly List<(T item, float priority)> items = new();

        public int Count => items.Count;

        public void Enqueue(T item, float priority) {
            items.Add((item, priority));
        }

        public T Dequeue() {
            int bestIndex = 0;

            for(int i = 1; i < items.Count; i++) {
                if(items[i].priority < items[bestIndex].priority) {
                    bestIndex = i;
                }
            }

            T bestItem = items[bestIndex].item;
            items.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}