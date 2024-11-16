using PROG7312_ST10023767.Controllers;
using PROG7312_ST10023767.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models.DataStructures
{
    public class TreeNode
    {
        public string Category { get; set; }
        public List<IssueClass> Issues { get; set; }
        public List<TreeNode> SubCategories { get; set; }

        public TreeNode(string category)
        {
            Category = category;
            Issues = new List<IssueClass>();
            SubCategories = new List<TreeNode>();
        }

        public void AddIssue(IssueClass issue)
        {
            Issues.Add(issue);
        }

        public void AddSubCategory(TreeNode subCategory)
        {
            SubCategories.Add(subCategory);
        }
    }

}
