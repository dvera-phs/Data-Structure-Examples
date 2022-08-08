using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Examples.DataStructures {
    public class Queue<T> : IEnumerable<T> {
        private LinkedList<T> List = new LinkedList<T>();
        public int Count => List.Count;

        // O(1) - insert an element into the queue (at the end)
        public void Enqueue(T Data) {
            List.AddLast(Data);
        }

        // O(1) - remove an element from the queue (from the front)
        public T Dequeue() {
            var value = List.GetFirst();
            List.RemoveFirst();
            return value;
        }

        // O(1) - get the first element in the queue (at the front)
        public T Peek() {
            return List.GetFirst();
        }

        #region IEnumerable Implementation
        public IEnumerator<T> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        #endregion
    }
}
