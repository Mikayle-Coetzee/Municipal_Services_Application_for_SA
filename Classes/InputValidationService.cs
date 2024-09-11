using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class InputValidationService
    {
        public static bool ValidateLocation(string location)
        {
            return !string.IsNullOrWhiteSpace(location);
        }

        public static bool ValidateDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
        }

        public static bool ValidateMediaUpload(List<string> mediaPaths)
        {
            return mediaPaths != null && mediaPaths.Any();
        }
    }
}
