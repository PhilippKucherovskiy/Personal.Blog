using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsAPIController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsAPIController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.TagId)
            {
                return BadRequest();
            }

            await _tagService.UpdateTagAsync(tag);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            await _tagService.CreateTagAsync(tag);
            return CreatedAtAction("GetTag", new { id = tag.TagId }, tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return NoContent();
        }
    }
}
