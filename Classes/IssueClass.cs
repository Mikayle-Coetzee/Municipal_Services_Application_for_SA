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
        public List<string> Attachments { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        public IssueClass() { }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        public IssueClass(string location, string category, string description)
        {
            Location = location;
            Category = category;
            Description = description;
            Attachments = new List<string>();
        }
        public void AddAttachment(string filePath)
        {
            if (File.Exists(filePath))
            {
                Attachments.Add(filePath);
            }
        }

    }
}
//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//