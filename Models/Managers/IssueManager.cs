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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Private tree structure that holds issues categorized by type.
        /// </summary>
        private TreeNode _issueTree;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Private Binary Search Tree to store issues.
        /// </summary>
        private BinarySearchTree _issueBST;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Private Max Heap used for retrieving issues based on priority.
        /// </summary>
        private MaxHeap _issueHeap;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all the issue categories as a HashSet of category names.
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetReportCategoryNames()
        {
            var categoryNames = new HashSet<string>();

            foreach (ReportCategory category in Enum.GetValues(typeof(ReportCategory)))
            {
                categoryNames.Add(category.ToString());
            }

            return categoryNames;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all issues by combining those from the tree, binary search tree, and max heap.
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves issues from the Binary Search Tree.
        /// </summary>
        /// <returns></returns>
        private List<IssueClass> GetIssuesFromBST()
        {
            var issues = new List<IssueClass>();
            InOrderTraversal(_issueBST.Root, issues);  
            return issues;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves issues from the Max Heap.
        /// </summary>
        /// <returns></returns>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// In-order traversal helper function to collect issues from the Binary Search Tree.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issues"></param>
        private void InOrderTraversal(BinarySearchTreeNode node, List<IssueClass> issues)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, issues);
                issues.Add(node.Issue);
                InOrderTraversal(node.Right, issues);
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves the most recent issues, up to a maximum of 5 issues.
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> GetRecentIssues()
        {
            var recentIssues = new List<IssueClass>();
            for (int i = 0; i < 5 && _issueHeap.Count() > 0; i++)
            {
                recentIssues.Add(_issueHeap.ExtractMax());
            }
            return recentIssues;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a new issue to all the data structures: Tree, Binary Search Tree, and Max Heap.
        /// </summary>
        /// <param name="issue"></param>
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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves issues sorted by timestamp from the Binary Search Tree.
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> GetIssuesSortedByTimestamp()
        {
            return _issueBST.InOrderTraversal();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves the latest issue based on the Max Heap priority.
        /// </summary>
        /// <returns></returns>
        public IssueClass GetLatestIssue()
        {
            return _issueHeap.ExtractMax();
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

