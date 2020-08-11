using System;

namespace BloggingPlatform.Models
{
    public class BlogPostModel
    {
        public static readonly string dateFormat = "yyyy-mm-ddThh:mm:ss.fffZ";
        public string slug { get; set; } = "";
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public string body { get; set; } = "";
        public string[] tagList { get; set; } = new string[0];
        public string createdAt { get; set; } = DateTime.UtcNow.ToString(dateFormat);
        public string updatedAt { get; set; } = DateTime.UtcNow.ToString(dateFormat);

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
            createdAt = post.PostCreated.ToString(dateFormat);
            updatedAt = post.PostUpdated.ToString(dateFormat);
        }
    }
}
