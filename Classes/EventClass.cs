using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class EventClass
    {
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }

        public string EndTime { get; set; }
        public string EndDate { get; set; }

        public string Location { get; set; }
        public string Venue { get; set; }

        public string Description { get; set; }

        public List<MediaFileClass> MediaFiles { get; set; }

        public string Type { get; set; }
        public string Category { get; set; }

        public EventClass(string title, string startDate, string time, string location, string description,
            List<MediaFileClass> mediaFiles, string type, string category, string endDate, string venue, string endTime)
        {
            Title = title;
            StartDate = startDate;
            StartTime = time;
            Location = location;
            Description = description;
            MediaFiles = mediaFiles;
            Type = type;
            Category = category;
            EndDate = endDate;
            Venue = venue;
            EndTime = endTime;
        }
    }
}
