using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class AVLTreeNode
    {
        /// <summary>
        /// The issue associated with this node.
        /// </summary>
        public IssueClass Issue { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The left child of the current node.
        /// </summary>
        public AVLTreeNode Left { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The right child of the current node.
        /// </summary>
        public AVLTreeNode Right { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The height of this node, used for balancing the tree.
        /// </summary>
        public int Height { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="issue"></param>
        public AVLTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
            Height = 1;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
