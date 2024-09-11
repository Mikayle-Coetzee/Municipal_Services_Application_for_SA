using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class IssueManager
    {
        private List<IssueClass> issues;
        public enum ReportCategory
        {
            Utilities,
            Sanitation,
            Potholes,
            Traffic,
            RoadSigns,
            OtherIssue,
            TrafficLights,
            CarCrash
        }

        public IssueManager()
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

