using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class AVLTree
    {
        /// <summary>
        /// The root of the AVL tree.
        /// </summary>
        private AVLTreeNode root;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the root node of the AVL tree.
        /// </summary>
        public AVLTreeNode Root
        {
            get { return root; }
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Inserts a new issue into the AVL tree, maintaining the tree balance.
        /// </summary>
        /// <param name="issue"></param>
        public void Insert(IssueClass issue)
        {
            root = Insert(root, issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Helper method to insert an issue into the AVL tree.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="issue"></param>
        /// <returns></returns>
        private AVLTreeNode Insert(AVLTreeNode node, IssueClass issue)
        {
            if (node == null) return new AVLTreeNode(issue);

             if (issue.Timestamp < node.Issue.Timestamp)
                node.Left = Insert(node.Left, issue);
            else
                node.Right = Insert(node.Right, issue);

            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;

            int balance = GetBalance(node);

            // Balance the tree
            if (balance > 1 && issue.Timestamp < node.Left.Issue.Timestamp) return RightRotate(node);
            if (balance < -1 && issue.Timestamp > node.Right.Issue.Timestamp) return LeftRotate(node);
            if (balance > 1 && issue.Timestamp > node.Left.Issue.Timestamp) { node.Left = LeftRotate(node.Left); return RightRotate(node); }
            if (balance < -1 && issue.Timestamp < node.Right.Issue.Timestamp) { node.Right = RightRotate(node.Right); return LeftRotate(node); }

            return node;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the height of a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int GetHeight(AVLTreeNode node)
        {
            return node == null ? 0 : node.Height;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets the balance factor of a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int GetBalance(AVLTreeNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Performs a right rotation on the given node.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private AVLTreeNode RightRotate(AVLTreeNode y)
        {
            AVLTreeNode x = y.Left;
            AVLTreeNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Performs a left rotation on the given node.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private AVLTreeNode LeftRotate(AVLTreeNode x)
        {
            AVLTreeNode y = x.Right;
            AVLTreeNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
