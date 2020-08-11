using Slugify;
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

        public Post(BlogPostModel post)
        {
            SlugHelper helper = new SlugHelper();
            PostIdentifier = post.slug;
            PostTitle = post.title;
            PostDescription = post.description;
            PostBody = post.body;
            PostCreated = DateTime.UtcNow;
            PostUpdated = DateTime.UtcNow;
        }

        public void updateFields (BlogPostModel post)
        {
            //SlugHelper helper = new SlugHelper();
            PostTitle = updateField(PostTitle, post.title);
            PostIdentifier = updateField(PostIdentifier, post.slug);
            PostDescription = updateField(PostDescription, post.description);
            PostBody = updateField(PostBody, post.body);
            PostUpdated = DateTime.UtcNow;
        }

        private string updateField (string field, string update)
        {
            if (update == null || update == "") return field;
            return update;
        }
    }
}
