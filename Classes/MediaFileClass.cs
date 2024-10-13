using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_ST10023767.Classes
{
    public class MediaFileClass
    {
        public string FileName { get; set; }   
        public byte[] FileContent { get; set; } 
        public string MediaType { get; set; }   

        public MediaFileClass(string fileName, byte[] fileContent, string mediaType)
        {
            FileName = fileName;
            FileContent = fileContent;
            MediaType = mediaType;
        }
    }
}
