using System;

namespace BloggingPlatform.Models
{
    public class BlogPostModel
    {
        public string slug { get; set; } = "";
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public string body { get; set; } = "";
        public string[] tagList { get; set; } = new string[0];
        public DateTime createdAt { get; set; } = DateTime.UtcNow.ToUniversalTime();
        public DateTime updatedAt { get; set; } = DateTime.UtcNow.ToUniversalTime();

        public BlogPostModel()
        {

        }

        public BlogPostModel (Post post, string[] tagArray)
        {
            slug = post.PostIdentifier;
            title = post.PostTitle;
            description = post.PostDescription;
            body = post.PostBody;
            tagList = tagArray;
            createdAt = post.PostCreated;
            updatedAt = post.PostUpdated;
        }
    }
}
