using System;
using System.Collections.Generic;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class Graph<T>
    {
        /// <summary>
        /// A dictionary that stores each vertex as a key
        /// </summary>
        private Dictionary<T, List<T>> _adjacencyList;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of the Graph class with an empty adjacency list.
        /// </summary>
        public Graph()
        {
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a new vertex to the graph.
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertex(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList[vertex] = new List<T>();
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a directed edge from one vertex to another. 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddEdge(T from, T to)
        {
            if (!_adjacencyList.ContainsKey(from)) AddVertex(from);
            if (!_adjacencyList.ContainsKey(to)) AddVertex(to);

            _adjacencyList[from].Add(to);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// 
        /// </sum Retrieves a list of vertices that are directly connected to the given vertex.mary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<T> GetConnections(T vertex)
        {
            return _adjacencyList.ContainsKey(vertex) ? _adjacencyList[vertex] : new List<T>();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves the entire graph as a dictionary of vertices and their connected vertices.
        /// </summary>
        /// <returns></returns>
        public Dictionary<T, List<T>> GetGraph()
        {
            return _adjacencyList;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
