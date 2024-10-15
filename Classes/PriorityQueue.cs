using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// A list that holds tuples of elements and their associated priorities
        /// </summary>
        private List<(TElement Element, TPriority Priority)> _elements = new List<(TElement, TPriority)>();

        /// <summary>
        /// Gets the number of elements in the priority queue
        /// </summary>
        public int Count => _elements.Count;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds an element to the priority queue with the specified priority
        /// </summary>
        /// <param name="element"></param>
        /// <param name="priority"></param>
        public void Enqueue(TElement element, TPriority priority)
        {
            _elements.Add((element, priority));
            HeapifyUp(_elements.Count - 1);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Removes and returns the element with the highest priority (lowest priority value) from the queue.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public TElement Dequeue()
        {
            if (_elements.Count == 0) throw new InvalidOperationException("The queue is empty.");

            var root = _elements[0].Element;

            _elements[0] = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);
            HeapifyDown(0);

            return root;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Restores the heap property by moving the element at the given index up to its correct position
        /// </summary>
        /// <param name="index"></param>
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_elements[index].Priority.CompareTo(_elements[parentIndex].Priority) >= 0)
                {
                    break;
                }

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Restores the heap property by moving the element at the given index down to its correct position
        /// </summary>
        /// <param name="index"></param>
        private void HeapifyDown(int index)
        {
            while (index < _elements.Count)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int smallestIndex = index;

                if (leftChildIndex < _elements.Count &&
                    _elements[leftChildIndex].Priority.CompareTo(_elements[smallestIndex].Priority) < 0)
                {
                    smallestIndex = leftChildIndex;
                }

                if (rightChildIndex < _elements.Count &&
                    _elements[rightChildIndex].Priority.CompareTo(_elements[smallestIndex].Priority) < 0)
                {
                    smallestIndex = rightChildIndex;
                }

                if (smallestIndex == index)
                {
                    break;
                }

                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Swaps the elements at the specified indices in the heap
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void Swap(int index1, int index2)
        {
            var temp = _elements[index1];
            _elements[index1] = _elements[index2];
            _elements[index2] = temp;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//