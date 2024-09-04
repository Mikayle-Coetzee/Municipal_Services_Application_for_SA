using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class IssueManagerClass
    {
        private List<IssueClass> issues;

        public IssueManagerClass()
        {
            issues = new List<IssueClass>();
        }

        public void AddIssue(IssueClass issue)
        {
            issues.Add(issue);
        }

        public List<IssueClass> GetIssues()
        {
            return issues;
        }
    }
}

