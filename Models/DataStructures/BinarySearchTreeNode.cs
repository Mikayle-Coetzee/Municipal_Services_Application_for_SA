using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class BinarySearchTreeNode
    {
        /// <summary>
        /// The issue associated with this node in the tree.
        /// </summary>
        public IssueClass Issue { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The left child node of the current node.
        /// </summary>
        public BinarySearchTreeNode Left { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The right child node of the current node.
        /// </summary>
        public BinarySearchTreeNode Right { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Constructor to create a new node with the specified issue.
        /// </summary>
        /// <param name="issue"></param>
        public BinarySearchTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = null;
            Right = null;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
