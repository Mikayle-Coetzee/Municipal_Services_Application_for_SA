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
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public List<string> MediaFiles { get; set; }

        public string Type { get; set; }
        public string Category { get; set; }

        public EventClass(string title, string date, string time, string location, string description, List<string> mediaFiles
            , string type, string category)
        {
            Title = title;
            Date = date;
            Time = time;
            Location = location;
            Description = description;
            MediaFiles = mediaFiles;
            Type = type;
            Category = category;
        }
    }
}
