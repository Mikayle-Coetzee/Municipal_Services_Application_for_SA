using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class MaxHeap
    {
        /// <summary>
        /// The internal list that stores the elements of the heap.
        /// </summary>
        private List<IssueClass> heap = new List<IssueClass>();

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Gets the number of elements in the heap.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return heap.Count;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Inserts a new issue into the heap and maintains the heap property.
        /// </summary>
        /// <param name="issue"></param>
        public void Insert(IssueClass issue)
        {
            heap.Add(issue);
            HeapifyUp(heap.Count - 1);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Restores the heap property by moving an element up the heap until it is in the correct position.
        /// </summary>
        /// <param name="index"></param>
        private void HeapifyUp(int index)
        {
            while (index > 0 && heap[index].Timestamp > heap[(index - 1) / 2].Timestamp)
            {
                var temp = heap[index];
                heap[index] = heap[(index - 1) / 2];
                heap[(index - 1) / 2] = temp;

                index = (index - 1) / 2;
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Removes and returns the maximum element (root of the heap) and restores the heap property.
        /// </summary>
        /// <returns></returns>
        public IssueClass ExtractMax()
        {
            if (heap.Count == 0) return null;

            var max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            HeapifyDown(0);
            return max;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Restores the heap property by moving an element down the heap until it is in the correct position.
        /// </summary>
        /// <param name="index"></param>
        private void HeapifyDown(int index)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            int largest = index;
            if (leftChild < heap.Count && heap[leftChild].Timestamp > heap[largest].Timestamp)
                largest = leftChild;
            if (rightChild < heap.Count && heap[rightChild].Timestamp > heap[largest].Timestamp)
                largest = rightChild;

            if (largest != index)
            {
                var temp = heap[index];
                heap[index] = heap[largest];
                heap[largest] = temp;

                HeapifyDown(largest);
            }
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
