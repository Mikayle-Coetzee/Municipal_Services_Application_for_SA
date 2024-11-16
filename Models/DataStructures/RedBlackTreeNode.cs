using PROG7312_ST10023767.Controllers;
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
        public IssueClass Issue { get; set; }
        public RedBlackTreeNode Left { get; set; }
        public RedBlackTreeNode Right { get; set; }
        public bool IsRed { get; set; }

        public RedBlackTreeNode(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
            IsRed = true;  
        }
    }

}
