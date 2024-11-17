using PROG7312_ST10023767.Models.DataStructures;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.Managers
{
    /// <summary>
    /// Manages the collection of issues reported by the user
    /// </summary>
    public class IssueManager
    {
        /// <summary>
        /// Represents different categories of reports that users can select when reporting an issue
        /// </summary>
        public enum ReportCategory
        {
            [Description("Utilities")]
            Utilities,

            [Description("Sanitation")]
            Sanitation,

            [Description("Potholes")]
            Potholes,

            [Description("Traffic")]
            Traffic,

            [Description("Road Signs")]
            RoadSigns,

            [Description("Other Issue")]
            OtherIssue,

            [Description("Traffic Lights")]
            TrafficLights,

            [Description("Car Crash")]
            CarCrash
        }

        public static HashSet<string> GetReportCategoryNames()
        {
            var categoryNames = new HashSet<string>();

            foreach (ReportCategory category in Enum.GetValues(typeof(ReportCategory)))
            {
                categoryNames.Add(category.ToString());
            }

            return categoryNames;
        }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public IssueManager()
        {
            // Initialize the data structures
            _issueTree = new TreeNode("All Issues");
            _issueBST = new BinarySearchTree();
            _issueHeap = new MaxHeap();
        }

        /// <summary>
        /// Retrieves all issues from the tree structure by category
        /// </summary>
        /// <returns>List of issues</returns>
        public List<IssueClass> GetIssues()
        {
            var allIssuesSet = new HashSet<IssueClass>();   

            // Retrieve issues from the tree structure
            foreach (var category in _issueTree.SubCategories)
            {
                foreach (var issue in category.Issues)
                {
                    allIssuesSet.Add(issue);   
                }
            }

            // Retrieve issues from the binary search tree
            foreach (var issue in GetIssuesFromBST())
            {
                allIssuesSet.Add(issue);   
            }

            // Retrieve issues from the max heap
            foreach (var issue in GetIssuesFromHeap())
            {
                allIssuesSet.Add(issue);  
            }

            // Convert the HashSet to a List before returning
            return allIssuesSet.ToList();
        }

        private List<IssueClass> GetIssuesFromBST()
        {
            var issues = new List<IssueClass>();
            InOrderTraversal(_issueBST.Root, issues);  
            return issues;
        }

        public List<IssueClass> GetIssuesFromHeap()
        {
            var issues = new List<IssueClass>();
            var heapCopy = new MaxHeap();   
            while (_issueHeap.Count() > 0)
            {
                var issue = _issueHeap.ExtractMax();
                issues.Add(issue);
                heapCopy.Insert(issue);   
            }
            _issueHeap = heapCopy;  
            return issues;
        }



        private void InOrderTraversal(BinarySearchTreeNode node, List<IssueClass> issues)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, issues);
                issues.Add(node.Issue);
                InOrderTraversal(node.Right, issues);
            }
        }


        /// <summary>
        /// Retrieves the most recent issues in sorted order
        /// </summary>
        /// <returns>List of issues</returns>
        public List<IssueClass> GetRecentIssues()
        {
            var recentIssues = new List<IssueClass>();
            for (int i = 0; i < 5 && _issueHeap.Count() > 0; i++)
            {
                recentIssues.Add(_issueHeap.ExtractMax());
            }
            return recentIssues;
        }

        private TreeNode _issueTree;  
        private BinarySearchTree _issueBST;   
        private MaxHeap _issueHeap;   



        public void AddIssue(IssueClass issue)
        {
            // Add to Tree
            TreeNode categoryNode = _issueTree.SubCategories
                .FirstOrDefault(c => c.Category == issue.Category.ToString());

            if (categoryNode == null)
            {
                categoryNode = new TreeNode(issue.Category.ToString());
                _issueTree.AddSubCategory(categoryNode);
            }

            categoryNode.AddIssue(issue);

            // Add to Binary Search Tree
            _issueBST.Insert(issue);

            // Add to Max Heap
            _issueHeap.Insert(issue);
        }

        public List<IssueClass> GetIssuesSortedByTimestamp()
        {
            return _issueBST.InOrderTraversal();
        }

        public IssueClass GetLatestIssue()
        {
            return _issueHeap.ExtractMax();
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

