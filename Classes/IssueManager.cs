using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    /// <summary>
    /// Manages the collection of issues reported by the user
    /// </summary>
    public class IssueManager
    {
        /// <summary>
        /// Stores the list of issues reported by the user
        /// </summary>
        public List<IssueClass> issues;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Represents different categories of reports that users can select when reporting an issue
        /// </summary>
        public enum ReportCategory
        {
            [Description("Utilities")]
            Utilities,

            [Description("Sanitation")]
            Sanitation,

            [Description("Potholes")]
            Potholes,

            [Description("Traffic")]
            Traffic,

            [Description("Road Signs")]
            RoadSigns,

            [Description("Other Issue")]
            OtherIssue,

            [Description("Traffic Lights")]
            TrafficLights,

            [Description("Car Crash")]
            CarCrash
        }


        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        public IssueManager()
        {
            issues = new List<IssueClass>();
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a new issue to the issue list
        /// </summary>
        /// <param name="issue"></param>
        public void AddIssue(IssueClass issue)
        {
            issues.Add(issue);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Retrieves the list of all issues that have been reported
        /// </summary>
        /// <returns></returns>
        public List<IssueClass> GetIssues()
        {
            return issues;
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//

