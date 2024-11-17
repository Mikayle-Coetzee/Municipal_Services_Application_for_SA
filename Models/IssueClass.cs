using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models
{
    /// <summary>
    ///  Represents an issue that the user report
    /// </summary>
    public class IssueClass
    {
        /// <summary>
        /// Gets or sets the location where the issue was reported
        /// </summary>
        public string Location { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        ///  Gets or sets the category of the reported issue
        /// </summary>
        public string Category { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the description of the issue provided by the user
        /// </summary>
        public string Description { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the timestamp when the issue was created or reported
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the status of the report posted, like 1= pendin
        /// </summary>
        public string Status { get; set; }


        public Guid IssueID { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// A list that holds the paths of media files
        /// </summary>
        public List<string> Attachments = new List<string>();

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="location"></param>
        /// <param name="category"></param>
        /// <param name="description"></param>
        public IssueClass(string location, string category, string description, string status)
        {
            IssueID = Guid.NewGuid();
            Location = location;
            Category = category;
            Description = description;
            Timestamp = DateTime.Now;
            Status = status;
        }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Adds a media attachment to the issue's list 
        /// </summary>
        /// <param name="media"></param>
        public void AddAttachment(string media)
        {
            Attachments.Add(media);
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//