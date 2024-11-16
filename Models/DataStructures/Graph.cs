using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class Graph
    {
        private Dictionary<string, List<IssueClass>> adjacencyList = new Dictionary<string, List<IssueClass>>();

        public void AddEdge(string category, IssueClass issue)
        {
            if (!adjacencyList.ContainsKey(category))
                adjacencyList[category] = new List<IssueClass>();

            adjacencyList[category].Add(issue);
        }

        public List<IssueClass> GetAdjacentIssues(string category)
        {
            return adjacencyList.ContainsKey(category) ? adjacencyList[category] : new List<IssueClass>();
        }
    }

}
