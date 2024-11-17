using PROG7312_ST10023767.Models;
using PROG7312_ST10023767.Models.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

                issue.Attachments = new List<string>();

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


        public void AddDummyEvents(PostManager postManager)
        {
            // Dummy data for events
            var titles = new List<string>
            {
                "Local Music Festival",
                "Art Exhibition",
                "Theater Play Premiere",
                "City Marathon",
                "Tech Networking Event",
                "Community Announcement"
            };

            var locations = new List<string>
            {
                "Claremont",
                "Art Gallery",
                "Local Theater",
                "Langebaan",
                "Convention Center",
                "Online"
            };

            var venues = new List<string>
            {
                "Grand Arena",
                "Studio Room A",
                "Main Stage",
                "Start Line",
                "Conference Hall",
                "Zoom Webinar"
            };

            var descriptions = new List<string>
            {
                "A night of live music and fun.",
                "Explore the creativity of local artists.",
                "Experience a captivating theatrical performance.",
                "Join the annual marathon with participants from all over.",
                "Meet professionals and grow your network.",
                "An important announcement for the community."
            };

            var categories = new List<string>
            {
                "Music",
                "Art",
                "Theater",
                "Sports",
                "Networking",
                "Other"
            };

            var types = new List<string>
            {
                "Event",
                "Announcement"
            };

            Random rand = new Random();

            // Generate 10 dummy events
            for (int i = 0; i < 10; i++)
            {
                string title = titles[rand.Next(titles.Count)];
                string location = locations[rand.Next(locations.Count)];
                string venue = venues[rand.Next(venues.Count)];
                string description = descriptions[rand.Next(descriptions.Count)];
                string category = categories[rand.Next(categories.Count)];
                string type = types[rand.Next(types.Count)];

                // Generate random dates
                DateTime startDate = DateTime.Now.AddDays(rand.Next(-10, 10));
                DateTime endDate = startDate.AddHours(rand.Next(1, 48)); // Event lasts 1 to 48 hours
                string startTime = startDate.ToString("hh:mm tt");
                string endTime = endDate.ToString("hh:mm tt");

                // Create dummy event
                var dummyEvent = new EventClass(
                    title,
                    startDate.ToString("dd MMM yyyy"),
                    startTime,
                    location,
                    description,
                    new List<MediaFileClass>(), // Assuming no media files for dummy data
                    type,
                    category,
                    endDate.ToString("dd MMM yyyy"),
                    venue,
                    endTime
                );


                if (IsNewLocation(location, postManager))
                {
                    AddNewLocation(location, postManager);
                }
                AddCategoryToUniqueCategories(category, postManager);
                // Add to event list
                postManager.locationEvents[location].Add(dummyEvent);
            }
        }


        private bool IsNewLocation(string location, PostManager postManager)
        {
            return !postManager.locationEvents.ContainsKey(location);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a new location to the PostManager
        /// </summary>
        /// <param name="location"></param>
        private void AddNewLocation(string location, PostManager postManager)
        {
            postManager.locationEvents[location] = new List<EventClass>();
        }

        private void AddCategoryToUniqueCategories(string category, PostManager postManager)
        {
            if (!string.IsNullOrEmpty(category))
            {
                postManager.uniqueCategories.Add(category);
            }
        }
    }
}
