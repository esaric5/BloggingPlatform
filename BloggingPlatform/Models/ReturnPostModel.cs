using System;

namespace BloggingPlatform.Models
{
    public class ReturnPostModel
    {
        public string slug;
        public string title;
        public string description;
        public string body;
        public string[] tagList;
        public DateTime createdAt;
        public DateTime updatedAt;

        public ReturnPostModel(Post post, string[] tagArray)
        {
            this.slug = post.PostIdentifier;
            this.title = post.PostTitle;
            this.description = post.PostDescription;
            this.body = post.PostBody;
            this.tagList = tagArray;
            this.createdAt = post.PostCreated;
            this.updatedAt = post.PostUpdated;
        }
    }
}
