using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class BinarySearchTree
    {
        /// <summary>
        /// The root node of the binary search tree.
        /// </summary>
        public BinarySearchTreeNode Root { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Constructor initializes the tree with a null root.
        /// </summary>
        public BinarySearchTree()
        {
            Root = null;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Inserts a new issue into the binary search tree.
        /// </summary>
        /// <param name="issue"></param>
        public void Insert(IssueClass issue)
        {
            Root = Insert(Root, issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// method to insert an issue into the tree.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issue"></param>
        /// <returns></returns>
        private BinarySearchTreeNode Insert(BinarySearchTreeNode node, IssueClass issue)
        {
            if (node == null)
            {
                return new BinarySearchTreeNode(issue);
            }

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

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Performs an in-order traversal of the tree and returns a list of issues.
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> InOrderTraversal()
        {
            var issues = new List<IssueClass>();
            InOrderTraversal(Root, issues);
            return issues;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Helper method for in-order traversal of the tree.
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
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
