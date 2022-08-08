using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Structure_Examples.DataStructures {

    // Why use a linkedlist?

    // This is the implementation of a List in .NET:
    // https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,cf7f4095e4de7646

    // Normal lists use an array implementation - meaning they have a fixed size of 16.
    // When an element is added exceeding the internal size of 16, all elements are
    // moved to a new internal list of size 32. The new internal list doubles in size 
    // whenever the maximum number of elements is exceeded. Whenever the List's internal
    // array is moved to a new array, each element must be moved one-by-one.

    // LinkedLists are different in that each element is not accessible directly by an index.
    // Because they are not dependent on being stored in a fixed location in memory, they do not
    // require a fixed size for their normal operations. They can be efficiently modified without
    // transferring to a new list.

    public class LinkedList<T> : IEnumerable<T> {
        private Node<T>? Root { get; set; }
        private Node<T>? Last { get; set; }
        public int Count { get; private set; }

        #region Get Methods

        // O(1) - get first element
        public T GetFirst() {
            if (Root == null) {
                throw new NullReferenceException("Could not get element from empty list.");
            }
            return Root.Data;
        }

        // O(1) - get first element
        public T GetLast() {
            if (Last == null) {
                throw new NullReferenceException("Could not get element from empty list.");
            }
            return Last.Data;
        }

        // O(n) - get element at specified location
        public T Get(int Index) {
            if (Index < 0 || Index >= Count) {
                throw new IndexOutOfRangeException();
            }

            var currentNode = Root;
            for (int i = 0; i < Index; i++) {
                currentNode = currentNode.Next;
            }

            return currentNode.Data;
        }

        #endregion

        #region Insert Methods

        // O(1) - insert at start
        public void AddFirst(T Data) {
            Root = new Node<T>(Data, Root);

            // if this is the only element in the list, root and last are the same reference
            if (Count == 0) {
                Last = Root;
            }

            Count++;
        }

        // O(1) - insert at end
        // This would be a very expensive O(n) operation if we did not maintain a reference to the last element
        public void AddLast(T Data) {
            var node = new Node<T>(Data);

            // if this is the only element in the list, root and last are the same reference
            if (Root == null) {
                Root = node;
                Last = node;
            } else {
                Last.Next = new Node<T>(Data); // add the node
                Last = Last.Next; // update the reference to the last element
            }

            Count++;
        }

        // O(1) - alias for AddLast
        public void Add(T Data) {
            AddLast(Data);
        }

        // O(n) - insert at any location
        public void Insert(T Data, int Index = 0) {
            if (Index > Count) {
                throw new IndexOutOfRangeException($"Invalid index '{Index}' for list with length {Count}.");
            }

            // if it's the first index, set the root as our data
            if (Index == 0) {
                AddFirst(Data);
            }

            // if it's the first index, set last as our data to save time iterating
            else if (Index == Count) {
                AddLast(Data);
            }

            // if it's any other index, insert a node between the previous and next nodes, and update their references
            else {
                var previousNode = Root;
                for (int i = 0; i < Index; i++) {
                    previousNode = previousNode.Next;
                }
                var nextNode = previousNode.Next; // store the next node so the value isn't lost when we update the reference
                previousNode.Next = new Node<T>(Data, nextNode);
                Count++;
            }
        }

        #endregion

        #region Remove Methods

        // O(1) - remove first element
        public void RemoveFirst() {
            if (Count == 0) {
                throw new NullReferenceException("Could not remove element from empty list.");
            }
            // if there is only one element in the list, both pointers would be removed
            if (Count == 1) {
                Root = null;
                Last = null;
            }
            // if there are multiple elements in the list, dereference the first element
            else {
                Root = Root.Next;
            }

            Count--;
        }

        // O(n) - remove last element
        // this is O(n) because to remove the last element, we need to dereference it from the previous node
        // we are not tracking the second-to-last node, so we have to iterate over everything to find it
        // this problem could be solved by tracking the second-to-last node, or by converting this into a
        // doubly linked list, and taking a step backward into the previous node
        public void RemoveLast() {

            if (Count == 0) {
                throw new NullReferenceException("Could not remove element from empty list.");
            } else if (Count == 1) {
                Root = null;
                Last = null;
            } else {

                // we need to track the node *before* the one we want to remove here,
                // so we iterate until we find a null node two nodes ahead of the current pointer
                var currentNode = Root;
                while (currentNode.Next.Next != null) {
                    currentNode = currentNode.Next;
                }
                currentNode.Next = null;
            }
            Count--;
        }

        // O(n) - remove element at any index
        public void Remove(int Index) {
            if (Index < 0 || Index >= Count) {
                throw new IndexOutOfRangeException();
            }

            if (Index == 0) {
                RemoveFirst();
                return;
            }

            // find node before the target element to remove
            var currentNode = Root;
            for (int i = 1; i < Index; i++) {
                currentNode = currentNode.Next;
            }

            // remove the target element by updating the previous node's reference
            if (currentNode.Next == Last) {
                currentNode.Next = null;
                Last = currentNode;
            } else {
                currentNode.Next = currentNode.Next.Next;
            }

        }

        #endregion

        #region Node Definition
        private class Node<TElement> {
            public TElement Data { get; set; }
            public Node<TElement>? Next { get; set; }

            public Node(TElement Data, Node<TElement>? Next = null) {
                this.Data = Data;
                this.Next = Next;
            }
        }

        #endregion

        #region IEnumerator Implementation for Foreach Support
        public IEnumerator<T> GetEnumerator() => new LinkedListEnumerator<T>(Root);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class LinkedListEnumerator<TElement> : IEnumerator<TElement> {

            private Node<TElement>? Root;
            private Node<TElement>? CurrentNode;
            public TElement Current {
                get {
                    if (!StartedIteration || CompletedIteration) {
                        throw new NotSupportedException("Could not retrieve current element");
                    } else {
                        Console.WriteLine("Got element");
                        return CurrentNode.Data;
                    }
                }
            }

            object IEnumerator.Current => Current;

            bool StartedIteration = false;
            bool CompletedIteration = false;

            public LinkedListEnumerator(Node<TElement>? Root) {
                this.Root = Root;
                CurrentNode = Root;
            }

            public bool MoveNext() {
                if (!StartedIteration) {
                    StartedIteration = true;
                    return StartedIteration;
                } else if (!CompletedIteration) {
                    CurrentNode = CurrentNode?.Next;
                    CompletedIteration = CurrentNode == null;
                    return !CompletedIteration;
                }
                throw new NotSupportedException("Could not move to next element after completing iteration");
            }

            public void Reset() {
                StartedIteration = false;
                CompletedIteration = false;
                CurrentNode = Root;
            }

            public void Dispose() { }
        }
        #endregion
    }

}
