using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class BinarySearchTreeNode
    {
        public IssueClass Issue { get; set; }
        public BinarySearchTreeNode Left { get; set; }
        public BinarySearchTreeNode Right { get; set; }

        public BinarySearchTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = null;
            Right = null;
        }
    }

    public class BinarySearchTree
    {
        public BinarySearchTreeNode Root { get; set; }

        public BinarySearchTree()
        {
            Root = null;
        }

         public void Insert(IssueClass issue)
        {
            Root = Insert(Root, issue);
        }

        private BinarySearchTreeNode Insert(BinarySearchTreeNode node, IssueClass issue)
        {
            if (node == null)
            {
                return new BinarySearchTreeNode(issue);
            }

            // Assuming comparison by issue timestamp for simplicity
            if (issue.Timestamp < node.Issue.Timestamp)
            {
                node.Left = Insert(node.Left, issue);
            }
            else
            {
                node.Right = Insert(node.Right, issue);
            }

            return node;
        }

         public List<IssueClass> InOrderTraversal()
        {
            var issues = new List<IssueClass>();
            InOrderTraversal(Root, issues);
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
    }


}
