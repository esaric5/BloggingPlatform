using System;
using System.Collections.Generic;

namespace BloggingPlatform.Models
{
    public partial class Tag
    {
        public Tag()
        {
            PostTag = new HashSet<PostTag>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }

        public ICollection<PostTag> PostTag { get; set; }
    }
}
