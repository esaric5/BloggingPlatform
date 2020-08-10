using System;
using System.Collections.Generic;

namespace BloggingPlatform.Models
{
    public partial class Post
    {
        public Post()
        {
            PostTag = new HashSet<PostTag>();
        }

        public int PostId { get; set; }
        public string PostIdentifier { get; set; }
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        public string PostBody { get; set; }
        public DateTime PostCreated { get; set; }
        public DateTime PostUpdated { get; set; }

        public ICollection<PostTag> PostTag { get; set; }
    }
}
