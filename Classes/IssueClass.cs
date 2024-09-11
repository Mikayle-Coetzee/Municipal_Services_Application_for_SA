using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class IssueClass
    {
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        
        public List<string> Attachments = new List<string>();

        public IssueClass(string location, string category, string description)
        {
            Location = location;
            Category = category;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public void AddAttachment(string media)
        {
            Attachments.Add(media);
        }
    }

}
//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//