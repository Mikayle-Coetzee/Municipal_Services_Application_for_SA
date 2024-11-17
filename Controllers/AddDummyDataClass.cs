using PROG7312_ST10023767.Models;
using PROG7312_ST10023767.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Controllers
{
    public class AddDummyDataClass
    {
        /// <summary>
        /// Adds dummy issues to the IssueManager.
        /// </summary>
        /// <param name="issueManager">The IssueManager instance where issues will be added.</param>
        public void AddDummyIssues(IssueManager issueManager, IssueTracker issueTracker)
        {
            // Dummy data
            var locations = new List<string> { "Claremont", "Bergvliet", "Langebaan", "Stellenbosch", "Mayville" };
            var categories = Enum.GetValues(typeof(IssueManager.ReportCategory));
            var descriptions = new List<string>
            {
                "Water leak reported in the area.",
                "Pothole causing traffic disruption.",
                "Street light not working.",
                "Traffic jam due to car crash."
            };

            // Generate 10 dummy issues
            Random rand = new Random();  
            for (int i = 0; i < 10; i++)
            {
                string location = locations[rand.Next(locations.Count)];
                string category = categories.GetValue(rand.Next(categories.Length)).ToString();
                string description = descriptions[rand.Next(descriptions.Count)];
                int status = rand.Next(0, 3); // Status: 0 (Pending), 1 (In Progress), 2 (Resolved)

                DateTime timestamp = GenerateRandomTimestamp(rand);  

                var issue = new IssueClass(location, category, description, status.ToString())
                {
                    Timestamp = timestamp 
                };

                issue.AddAttachment("report_" + i + ".pdf");

                issueManager.AddIssue(issue);
                issueTracker.AddIssue(issue);
            }
        }

        /// <summary>
        /// Generates a random timestamp within the past month.
        /// </summary>
        /// <returns>A random DateTime value.</returns>
        private DateTime GenerateRandomTimestamp(Random rand)
        {
            DateTime today = DateTime.Now;
            int daysAgo = rand.Next(0, 30);  
            DateTime randomDate = today.AddDays(-daysAgo);

            // Randomize the time within that day
            int hour = rand.Next(0, 24);    
            int minute = rand.Next(0, 60);   
            int second = rand.Next(0, 60);   

            return new DateTime(randomDate.Year, randomDate.Month, randomDate.Day, hour, minute, second);
        }
    }
}
