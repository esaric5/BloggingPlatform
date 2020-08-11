using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingPlatform.Models;

namespace BloggingPlatform.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly BloggingPlatformContext context;

        public TagsController(BloggingPlatformContext context)
        {
            this.context = context;
        }

        // GET: api/tags
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            string[] allTags = await context.Tag
                                    .OrderByDescending(tag => tag.TagName)
                                    .Select(tag => tag.TagName)
                                    .ToArrayAsync();
            return Ok(new
            {
                tags = allTags
            });
        }

    }
}