using System;
using System.Collections.Generic;

namespace BloggingPlatform.Models
{
    public partial class PostTag
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
