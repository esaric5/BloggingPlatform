using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingPlatform.Models;
using Newtonsoft.Json;
using Slugify;

namespace BloggingPlatform.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BloggingPlatformContext context;

        public PostsController(BloggingPlatformContext context)
        {
            this.context = context;
        }

        // GET: api/posts/:slug
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPost([FromRoute] string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var singlePost = await context.Post
                        .Where(post => post.PostIdentifier == slug)
                        .Select(post => new BlogPostModel(post, post.PostTag.Select(postTag => postTag.Tag.TagName).ToArray()))
                        .FirstOrDefaultAsync();

            if (singlePost == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                blogPost = singlePost
            });
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetPosts(string tag = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var posts = await context.Post
                        .OrderByDescending(post => post.PostUpdated)
                        .Where(post => tag==null || post.PostTag.Select(postTag => postTag.Tag.TagName).Contains(tag))
                        .Select(post => new BlogPostModel(post, post.PostTag.Select(postTag => postTag.Tag.TagName).ToArray()))
                        .ToArrayAsync();

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                blogPosts = posts,
                postsCount = posts.Length
            });
        }

        // PUT: api/posts/:slug
        [HttpPut("{slug}")]
        public async Task<IActionResult> PutPost([FromRoute] string slug, [FromBody] BlogPostModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ValidateSlug(ref post);
            Post postForUpdate = await context.Post
                                    .Where(blogPost => blogPost.PostIdentifier == slug)
                                    .SingleOrDefaultAsync();

            if (postForUpdate == null || 
                (post.title=="" && post.description=="" && post.body==""))
            {
                return BadRequest();
            }

            postForUpdate.updateFields(post);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(slug))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            BlogPostModel updatedPost = await context.Post
                                        .Where(blogPost => blogPost.PostId == postForUpdate.PostId)
                                        .Select(blogPost => new BlogPostModel(blogPost, blogPost.PostTag.Select(postTag => postTag.Tag.TagName).ToArray()))
                                        .SingleOrDefaultAsync();
            return Ok(updatedPost);
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> PostPost([FromBody] BlogPostModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidateSlug(ref post);
            Post newPost = new Post(post);
            context.Post.Add(newPost);

            foreach (var tag in post.tagList)
            {
                Tag newTag = await context.Tag.SingleOrDefaultAsync(postTag => postTag.TagName == tag);

                if (newTag == null)
                {
                    newTag = new Tag
                    {
                        TagName = tag
                    };
                    context.Tag.Add(newTag);
                }

                context.PostTag.Add(new PostTag
                {
                    Post = newPost,
                    Tag = newTag
                });
            }

            await context.SaveChangesAsync();

            return Ok(new BlogPostModel(newPost, post.tagList));
        }

        // DELETE: api/posts/:slug
        [HttpDelete("{slug}")]
        public async Task<IActionResult> DeletePost([FromRoute] string slug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = await context.Post.SingleOrDefaultAsync(blogPost => blogPost.PostIdentifier == slug);

            if (post == null)
            {
                return NotFound();
            }

            int[] ids = context.PostTag
                        .Where(postTag => postTag.PostId == post.PostId)
                        .Select(postTag => postTag.TagId)
                        .ToArray();

            foreach (int id in ids)
            {
                if (!context.PostTag.Any(postTag => postTag.TagId == id && postTag.PostId != post.PostId))
                {
                    context.Tag.Remove(context.Tag.SingleOrDefault(postTag => postTag.TagId == id));
                }
            }

            context.PostTag.RemoveRange(context.PostTag.Where(postTag => postTag.PostId == post.PostId));
            context.Post.Remove(post);
            await context.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool PostExists(string slug)
        {
            return context.Post.Any(post => post.PostIdentifier == slug);
        }

        private void ValidateSlug(ref BlogPostModel post)
        {
            string title = post.title;
            int count = context.Post.Count(blogPost => blogPost.PostTitle == title);
            int num = context.Post
                        .OrderByDescending(blogPost => blogPost.PostId)
                        .Select(blogPost => blogPost.PostId)
                        .FirstOrDefault();

            SlugHelper helper = new SlugHelper();
            if (count > 0) title+=" " + ++num;
            post.slug = helper.GenerateSlug(title);
        }
    }
}