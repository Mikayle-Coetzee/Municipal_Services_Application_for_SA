using PROG7312_ST10023767.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    internal class FilterAndRecommendHelper
    {
        /// <summary>
        /// Recommends events based on the user's search history
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<EventClass> RecommendEventsBasedOnSearch(string userId, Dictionary<string,
            List<string>> userSearchHistory, SortedDictionary<string, List<EventClass>> locationEvents)
        {
            // If the user has no search historu, it will return an empty new list
            if (!userSearchHistory.ContainsKey(userId) || userSearchHistory[userId].Count == 0)
            {
                return new List<EventClass>();
            }

            List<string> pastSearches = userSearchHistory[userId];

            // This will Get the top 3 most frequent search terms of the user
            var frequentSearches = pastSearches.GroupBy(search => search)
                                               .OrderByDescending(group => group.Count())
                                               .Select(group => group.Key)
                                               .Take(3)
                                               .ToList();

            // I made use of a custom Priority Queue so that its eaiser to mark
            PriorityQueue<EventClass, int> eventQueue = new PriorityQueue<EventClass, int>();

            foreach (var searchTerm in frequentSearches)
            {
                // For the weight, I made it to count the amount of searches that tearm has, and that will be the priority 
                int weight = pastSearches.Count(s => s.Equals(searchTerm));

                // This will find the events that match the search term
                var matchingEvents = locationEvents.Values.SelectMany(evList => evList)
                    .Where(ev =>
                        ev.Location.ToLower().Contains(searchTerm) ||
                        ev.Title.ToLower().Contains(searchTerm) ||
                        ev.Category.ToLower().Contains(searchTerm) ||
                        ev.Venue.ToLower().Contains(searchTerm) ||
                        ev.EndDate.ToLower().Contains(searchTerm) ||
                        ev.StartDate.ToLower().Contains(searchTerm) ||
                        ev.StartTime.ToLower().Contains(searchTerm) ||
                        ev.EndTime.ToLower().Contains(searchTerm))
                    .Distinct()
                    .ToList();

                foreach (var ev in matchingEvents)
                {
                    eventQueue.Enqueue(ev, weight);
                }
            }

            var currentDateTime = DateTime.Now;
            var upcomingAndOngoingEvents = new List<EventClass>();

            while (eventQueue.Count > 0)
            {
                var ev = eventQueue.Dequeue();

                if (IsBusyEvent(ev, currentDateTime) || DateTime.Parse(ev.StartDate) > currentDateTime)
                {
                    upcomingAndOngoingEvents.Add(ev);
                }
            }

            return upcomingAndOngoingEvents;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Tracks the user's search query in the search history.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchQuery"></param>
        public void TrackUserSearch(string userId, string searchQuery, Dictionary<string,
            List<string>> userSearchHistory)
        {
            if (!userSearchHistory.ContainsKey(userId))
            {
                userSearchHistory[userId] = new List<string>();
            }

            userSearchHistory[userId].Add(searchQuery.ToLower());
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Filters events by category using a category filter window
        /// </summary>
        /// <param name="eventsToDisplay"></param>
        /// <returns></returns>
        public List<EventClass> FilterByCategory(List<EventClass> eventsToDisplay, HashSet<string> uniqueCategories)
        {
            CategoryFilterWindow categoryWindow = new CategoryFilterWindow();
            categoryWindow.PopulateCategories(uniqueCategories);

            if (categoryWindow.ShowDialog() == true)
            {
                string selectedCategory = categoryWindow.SelectedCategory;
                return eventsToDisplay.Where(item => item.Category == selectedCategory).ToList();
            }

            return eventsToDisplay;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Checks if the event is in the past
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public bool IsPastEvent(EventClass item, DateTime currentDateTime)
        {
            DateTime startDate = DateTime.Parse(item.StartDate);
            DateTime endDate = DateTime.Parse(item.EndDate);

            return currentDateTime > startDate && currentDateTime > endDate && currentDateTime >= endDate.Date.AddDays(1).AddTicks(-1);
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Checks if the event is ongoing (i.e., busy)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public bool IsBusyEvent(EventClass item, DateTime currentDateTime)
        {
            DateTime startDate = DateTime.Parse(item.StartDate);
            DateTime endDate = DateTime.Parse(item.EndDate);

            return currentDateTime >= startDate && currentDateTime <= endDate.Date.AddDays(1).AddTicks(-1);
        }

    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
