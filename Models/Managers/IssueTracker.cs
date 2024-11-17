using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models.DataStructures;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;

namespace PROG7312_ST10023767.Models.Managers
{
    public class IssueTracker
    {
         private AVLTree _avlTree;
        private RedBlackTreeNode _redBlackTree;
        private BinarySearchTree _binarySearchTree;
        private Graph<IssueClass> _issueGraph;

        public IssueTracker()
        {
            _avlTree = new AVLTree();
            _redBlackTree = new RedBlackTreeNode(null);  
            _binarySearchTree = new BinarySearchTree();
            _issueGraph = new Graph<IssueClass>();

        }

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

        public void AddDependency(IssueClass from, IssueClass to)
        {
            _issueGraph.AddEdge(from, to);
        }

        public List<IssueClass> GetDependencies(IssueClass issue)
        {
            return _issueGraph.GetConnections(issue);
        }

        public Dictionary<IssueClass, List<IssueClass>> GetAllDependencies()
        {
            return _issueGraph.GetGraph();
        }
        public List<IssueClass> GetIssuesFromAVLTree()
        {
            var issues = new List<IssueClass>(); 
            InOrderTraversalAVL(_avlTree.Root, issues);
            return issues;
        }

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

         private List<IssueClass> GetIssuesFromBST()
        {
            return _binarySearchTree.InOrderTraversal();
        }

         private void InOrderTraversalAVL(AVLTreeNode node, List<IssueClass> issues)
        {
            if (node != null)
            {
                InOrderTraversalAVL(node.Left, issues);
                issues.Add(node.Issue);
                InOrderTraversalAVL(node.Right, issues);
            }
        }

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
}
