using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class BinaryTree
    {
        /// <summary>
        /// Gets or sets the IssueClass object stored in the current node.
        /// </summary>
        public IssueClass Issue { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the left child node in the binary tree.
        /// </summary>
        public BinaryTree Left { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the right child node in the binary tree.
        /// </summary>
        public BinaryTree Right { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of the BinaryTree class 
        /// </summary>
        /// <param name="issue"></param>
        public BinaryTree(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Inserts a new IssueClass object into the binary tree based on its timestamp.
        /// </summary>
        /// <param name="newIssue"></param>
        public void Insert(IssueClass newIssue)
        {
            if (newIssue.Timestamp < Issue.Timestamp)
            {
                if (Left == null) Left = new BinaryTree(newIssue);
                else Left.Insert(newIssue);
            }
            else
            {
                if (Right == null) Right = new BinaryTree(newIssue);
                else Right.Insert(newIssue);
            }
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
