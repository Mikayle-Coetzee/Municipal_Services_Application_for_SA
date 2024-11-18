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
        /// <summary>
        /// The category name for this node.
        /// </summary>
        public string Category { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  A list of issues associated with this category.
        /// </summary>
        public List<IssueClass> Issues { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  A list of subcategories under this category.
        /// </summary>
        public List<TreeNode> SubCategories { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Initializes a new instance of the TreeNode class with the specified category.
        /// </summary>
        /// <param name="category"></param>
        public TreeNode(string category)
        {
            Category = category;
            Issues = new List<IssueClass>();
            SubCategories = new List<TreeNode>();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds an issue to the list of issues for this category.
        /// </summary>
        /// <param name="issue"></param>
        public void AddIssue(IssueClass issue)
        {
            Issues.Add(issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a subcategory to this category. 
        /// </summary>
        /// <param name="subCategory"></param>
        public void AddSubCategory(TreeNode subCategory)
        {
            SubCategories.Add(subCategory);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
