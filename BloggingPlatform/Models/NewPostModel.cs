using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingPlatform.Models
{
    public class NewPostModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string body { get; set; }
        public string[] tagList { get; set; }
    }

}
