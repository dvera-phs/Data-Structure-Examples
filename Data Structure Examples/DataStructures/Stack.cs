using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Examples.DataStructures {
    public class Stack<T> : IEnumerable<T>{
        private LinkedList<T> List = new LinkedList<T>();
        public int Count => List.Count;

        // O(1) - insert an element on top of the stack
        public void Push(T Data) {
            List.AddFirst(Data);
        }

        // O(1) - remove the top element of the stack and return it
        public T Pop() {
            var value = List.GetFirst();
            List.RemoveFirst();
            return value;
        }

        // O(1) - get the top element of the stack
        public T Peek() {
            return List.GetFirst();
        }

        #region IEnumerable Implementation
        public IEnumerator<T> GetEnumerator() => List.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        #endregion
    }
}
