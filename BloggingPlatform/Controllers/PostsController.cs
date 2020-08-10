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
                        .Select(post => new ReturnPostModel(post, post.PostTag.Select(postTag => postTag.Tag.TagName).ToArray()))
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
                        .Select(post => new ReturnPostModel(post, post.PostTag.Select(postTag => postTag.Tag.TagName).ToArray()))
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

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost([FromRoute] int id, [FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostId)
            {
                return BadRequest();
            }

            context.Entry(post).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<IActionResult> PostPost([FromBody] NewPostModel post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SlugHelper helper = new SlugHelper();
            Post newPost = new Post
            {
                PostIdentifier = helper.GenerateSlug(post.title),
                PostTitle = post.title,
                PostDescription = post.description,
                PostBody = post.body,
                PostCreated = DateTime.Now,
                PostUpdated = DateTime.Now
            };

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

            return Ok(new ReturnPostModel(newPost, post.tagList));
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

            Console.Write("IDS for " + post.PostTitle + " "+ids.Length);
            foreach (int id in ids)
            {
                Console.Write(id + " ");
            }

            Console.WriteLine();
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

        private bool PostExists(int id)
        {
            return context.Post.Any(e => e.PostId == id);
        }
    }
}