using System;
using System.Collections.Generic;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class Graph<T>
    {
        private Dictionary<T, List<T>> _adjacencyList;

        public Graph()
        {
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        public void AddVertex(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<T>();
            }
        }

        public void AddEdge(T from, T to)
        {
            if (!_adjacencyList.ContainsKey(from)) AddVertex(from);
            if (!_adjacencyList.ContainsKey(to)) AddVertex(to);

            _adjacencyList[from].Add(to);
        }

        public List<T> GetConnections(T vertex)
        {
            return _adjacencyList.ContainsKey(vertex) ? _adjacencyList[vertex] : new List<T>();
        }

        public Dictionary<T, List<T>> GetGraph()
        {
            return _adjacencyList;
        }
    }
}
