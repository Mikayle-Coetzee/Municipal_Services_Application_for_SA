using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models.DataStructures;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;

namespace PROG7312_ST10023767.Models.Managers
{
    public class IssueTracker
    {
        /// <summary>
        /// AVL Tree to store issues
        /// </summary>
        private AVLTree _avlTree;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Red-Black Tree to store issues
        /// </summary>
        private RedBlackTreeNode _redBlackTree;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Binary Search Tree to store issues
        /// </summary>
        private BinarySearchTree _binarySearchTree;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Graph to store issue dependencies
        /// </summary>
        private Graph<IssueClass> _issueGraph;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor 
        /// </summary>
        public IssueTracker()
        {
            _avlTree = new AVLTree();
            _redBlackTree = new RedBlackTreeNode(null);
            _binarySearchTree = new BinarySearchTree();
            _issueGraph = new Graph<IssueClass>();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds an issue to all data structures
        /// </summary>
        /// <param name="issue"></param>
        public void AddIssue(IssueClass issue)
        {
            // Add issue to AVL Tree
            _avlTree.Insert(issue);

            // Add issue to Red-Black Tree  
            if (_redBlackTree != null)
            {
                InsertRedBlackTree(_redBlackTree, issue);
            }

            // Add issue to Binary Search Tree
            _binarySearchTree.Insert(issue);
            _issueGraph.AddVertex(issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Method to insert an issue into the Red-Black Tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issue"></param>
        private void InsertRedBlackTree(RedBlackTreeNode node, IssueClass issue)
        {

            node = new RedBlackTreeNode(issue);


            if (issue.Timestamp < node.Issue.Timestamp)
            {
                if (node.Left == null)
                {
                    node.Left = new RedBlackTreeNode(issue);
                }
                else
                {
                    InsertRedBlackTree(node.Left, issue);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new RedBlackTreeNode(issue);
                }
                else
                {
                    InsertRedBlackTree(node.Right, issue);
                }
            }

        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all issues across all data structures and combines them into a list
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> GetIssues()
        {
            var allIssues = new HashSet<IssueClass>();

            // Get issues from AVL Tree
            allIssues.UnionWith(GetIssuesFromAVLTree());

            // Get issues from Red-Black Tree
            allIssues.UnionWith(GetIssuesFromRedBlackTree(_redBlackTree));

            // Get issues from Binary Search Tree
            allIssues.UnionWith(GetIssuesFromBST());

            return new List<IssueClass>(allIssues);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a dependency between two issues in the Graph
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddDependency(IssueClass from, IssueClass to)
        {
            _issueGraph.AddEdge(from, to);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all dependent issues for a given issue
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public List<IssueClass> GetDependencies(IssueClass issue)
        {
            return _issueGraph.GetConnections(issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all issue dependencies in the graph
        /// </summary>
        /// <returns></returns>
        public Dictionary<IssueClass, List<IssueClass>> GetAllDependencies()
        {
            return _issueGraph.GetGraph();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Helper method to retrieve all issues from the AVL Tree using in-order traversal
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> GetIssuesFromAVLTree()
        {
            var issues = new List<IssueClass>();
            InOrderTraversalAVL(_avlTree.Root, issues);
            return issues;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all issues from the Red-Black Tree recursively
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<IssueClass> GetIssuesFromRedBlackTree(RedBlackTreeNode node)
        {
            var issues = new List<IssueClass>();
            if (node != null)
            {
                issues.Add(node.Issue);
                issues.AddRange(GetIssuesFromRedBlackTree(node.Left));
                issues.AddRange(GetIssuesFromRedBlackTree(node.Right));
            }
            return issues;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves all issues from the Binary Search Tree
        /// </summary>
        /// <returns></returns>
        private List<IssueClass> GetIssuesFromBST()
        {
            return _binarySearchTree.InOrderTraversal();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// In-order traversal for the AVL Tree to collect issues
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issues"></param>
        private void InOrderTraversalAVL(AVLTreeNode node, List<IssueClass> issues)
        {
            if (node != null)
            {
                InOrderTraversalAVL(node.Left, issues);
                issues.Add(node.Issue);
                InOrderTraversalAVL(node.Right, issues);
            }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Updates the status of an issue based on its ID and the new status
        /// </summary>
        /// <param name="issueID"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public bool UpdateIssueStatus(string issueID, string newStatus)
        {
            Guid issueGuid;
            if (Guid.TryParse(issueID, out issueGuid))
            {
                return UpdateStatusInAVL(_avlTree.Root, issueGuid, newStatus) ||
                       UpdateStatusInRedBlackTree(_redBlackTree, issueGuid, newStatus) ||
                       UpdateStatusInBST(_binarySearchTree.Root, issueGuid, newStatus);
            }

            return false;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Helper method to update the issue status in the AVL Tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issueID"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        private bool UpdateStatusInAVL(AVLTreeNode node, Guid issueID, string newStatus)
        {
            int statusValue = 0;

            if (newStatus == "Pending")
                statusValue = 0;
            else if (newStatus == "Active")
                statusValue = 1;
            else if (newStatus == "Resolved")
                statusValue = 2;

            if (node == null) return false;
            if (node.Issue.IssueID == issueID)
            {
                node.Issue.Status = statusValue.ToString();
                return true;
            }
            return issueID.CompareTo(node.Issue.IssueID) < 0 ?
                UpdateStatusInAVL(node.Left, issueID, newStatus) :
                UpdateStatusInAVL(node.Right, issueID, newStatus);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// elper method to update the issue status in the Red-Black Tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issueID"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        private bool UpdateStatusInRedBlackTree(RedBlackTreeNode node, Guid issueID, string newStatus)
        {
            int statusValue = 0;

            if (newStatus == "Pending")
                statusValue = 0;
            else if (newStatus == "Active")
                statusValue = 1;
            else if (newStatus == "Resolved")
                statusValue = 2;

            if (node == null) return false;
            if (node.Issue.IssueID == issueID)
            {
                node.Issue.Status = statusValue.ToString();
                return true;
            }
            return issueID.CompareTo(node.Issue.IssueID) < 0 ?
                UpdateStatusInRedBlackTree(node.Left, issueID, newStatus) :
                UpdateStatusInRedBlackTree(node.Right, issueID, newStatus);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Helper method to update the issue status in the Red-Black Tree
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issueID"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        private bool UpdateStatusInBST(BinarySearchTreeNode node, Guid issueID, string newStatus)
        {
            int statusValue = 0;

            if (newStatus == "Pending")
                statusValue = 0;
            else if (newStatus == "Active")
                statusValue = 1;
            else if (newStatus == "Resolved")
                statusValue = 2;

            if (node == null) return false;
            if (node.Issue.IssueID == issueID)
            {
                node.Issue.Status = statusValue.ToString();
                return true;
            }
            return issueID.CompareTo(node.Issue.IssueID) < 0 ?
                UpdateStatusInBST(node.Left, issueID, newStatus) :
                UpdateStatusInBST(node.Right, issueID, newStatus);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
