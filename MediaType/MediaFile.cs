using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media
{
    public class MediaFile
    {
        public string FilePath { get; set; }
        public MediaType Type { get; set; }
        public DateTime DateCreated { get; set; }

        public MediaFile(string filePath, MediaType type, DateTime dateCreated)
        {
            FilePath = filePath;
            Type = type;
            DateCreated = dateCreated;
        }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}
