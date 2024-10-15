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
        /// Stores the list of events posted by the user
        /// </summary>
        public SortedDictionary<string, List<EventClass>> locationEvents ;

        public Dictionary<string, List<string>> userSearchHistory ;

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
}
