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
        public IssueClass Issue { get; set; }
        public BinaryTree Left { get; set; }
        public BinaryTree Right { get; set; }

        public BinaryTree(IssueClass issue)
        {
            Issue = issue;
            Left = Right = null;
        }

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

}
