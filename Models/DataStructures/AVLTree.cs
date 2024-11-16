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
    public class AVLTreeNode
    {
        public IssueClass Issue { get; set; }
        public AVLTreeNode Left { get; set; }
        public AVLTreeNode Right { get; set; }
        public int Height { get; set; }

        public AVLTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
            Height = 1;
        }
    }

    public class AVLTree
    {
        private AVLTreeNode root;
        public AVLTreeNode Root
        {
            get { return root; }
        }

        public void Insert(IssueClass issue)
        {
            root = Insert(root, issue);
        }

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


        private int GetHeight(AVLTreeNode node)
        {
            return node == null ? 0 : node.Height;
        }

        private int GetBalance(AVLTreeNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

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

}
