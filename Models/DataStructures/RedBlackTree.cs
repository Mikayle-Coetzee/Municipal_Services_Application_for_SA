using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class RedBlackTree
    {
        /// <summary>
        /// The root node of the Red-Black Tree.
        /// </summary>
        private RedBlackTreeNode root;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public RedBlackTree()
        {
            root = null;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Inserts a new issue into the Red-Black Tree
        /// </summary>
        /// <param name="issue"></param>
        public void Insert(IssueClass issue)
        {
            RedBlackTreeNode newNode = new RedBlackTreeNode(issue);
            root = Insert(root, newNode);
            root.IsRed = false;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// method to insert a new node into the tree while maintaining the binary search tree property and fixing Red-Black Tree violations.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        private RedBlackTreeNode Insert(RedBlackTreeNode node, RedBlackTreeNode newNode)
        {
            if (node == null)
            {
                return newNode;
            }

            if (newNode.Issue.Timestamp < node.Issue.Timestamp)
            {
                node.Left = Insert(node.Left, newNode);
            }
            else
            {
                node.Right = Insert(node.Right, newNode);
            }

            // After insertion, fix any violations of the Red-Black Tree properties
            if (IsRed(node.Right) && !IsRed(node.Left)) node = RotateLeft(node);
            if (IsRed(node.Left) && IsRed(node.Left.Left)) node = RotateRight(node);
            if (IsRed(node.Left) && IsRed(node.Right)) FlipColors(node);

            return node;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Performs a left rotation 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private RedBlackTreeNode RotateLeft(RedBlackTreeNode node)
        {
            RedBlackTreeNode newRoot = node.Right;
            node.Right = newRoot.Left;
            newRoot.Left = node;
            newRoot.IsRed = node.IsRed;
            node.IsRed = true;
            return newRoot;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Performs a right rotation
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private RedBlackTreeNode RotateRight(RedBlackTreeNode node)
        {
            RedBlackTreeNode newRoot = node.Left;
            node.Left = newRoot.Right;
            newRoot.Right = node;
            newRoot.IsRed = node.IsRed;
            node.IsRed = true;
            return newRoot;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Flips the colors of a node
        /// </summary>
        /// <param name="node"></param>
        private void FlipColors(RedBlackTreeNode node)
        {
            node.IsRed = !node.IsRed;
            if (node.Left != null) node.Left.IsRed = !node.Left.IsRed;
            if (node.Right != null) node.Right.IsRed = !node.Right.IsRed;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Checks if the given node is red.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsRed(RedBlackTreeNode node)
        {
            return node != null && node.IsRed;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//