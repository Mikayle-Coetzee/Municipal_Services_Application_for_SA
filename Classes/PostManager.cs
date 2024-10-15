using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class PostManager
    {
        /// <summary>
        /// Stores the list of posts associated with different locations
        /// </summary>
        public SortedDictionary<string, List<EventClass>> locationEvents ;

        /// <summary>
        /// Keeps track of the search history for each user
        /// </summary>
        public Dictionary<string, List<string>> userSearchHistory ;

        /// <summary>
        /// Contains unique categories 
        /// </summary>
        public HashSet<string> uniqueCategories ;

        //・♫-------------------------------------------------------------------------------------------------♫・//
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PostManager()
        {
            locationEvents = new SortedDictionary<string, List<EventClass>>();
            userSearchHistory = new Dictionary<string, List<string>>();
            uniqueCategories = new HashSet<string>();
        }
    }
}//★---♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫---★・。。END OF FILE 。。・★---♫ ♬:;;;:♬ ♫:;;;: ♫ ♬:;;;:♬ ♫:;;;: ♫---★//
