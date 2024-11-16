using PROG7312_ST10023767.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Models
{
    public class EventClass
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public string StartDate { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the start time
        /// </summary>
        public string StartTime { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the end time
        /// </summary>
        public string EndTime { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        public string EndDate { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public string Location { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the venue
        /// </summary>
        public string Venue { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets a description
        /// </summary>
        public string Description { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the list of media files
        /// </summary>
        public List<MediaFileClass> MediaFiles { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string Type { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public string Category { get; set; }

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Defaulyy Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startDate"></param>
        /// <param name="time"></param>
        /// <param name="location"></param>
        /// <param name="description"></param>
        /// <param name="mediaFiles"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="endDate"></param>
        /// <param name="venue"></param>
        /// <param name="endTime"></param>
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
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
