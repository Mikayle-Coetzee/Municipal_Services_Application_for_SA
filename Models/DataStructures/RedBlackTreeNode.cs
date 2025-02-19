﻿using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class RedBlackTreeNode
    {
        /// <summary>
        /// The issue associated with this node. The node stores an IssueClass object.
        /// </summary>
        public IssueClass Issue { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// The left child of this node in the Red-Black tree.
        /// </summary>
        public RedBlackTreeNode Left { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  The right child of this node in the Red-Black tree.
        /// </summary>
        public RedBlackTreeNode Right { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// A flag indicating the color of the node. True means Red, False means Black.
        /// </summary>
        public bool IsRed { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of the RedBlackTreeNode class.
        /// </summary>
        /// <param name="issue"></param>
        public RedBlackTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
            IsRed = true;  
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
