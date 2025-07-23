using Microsoft.AspNetCore.Mvc;
using NotesApp.Server.Models;
using NotesApp.Server.Repositories;
using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAngularDevClient")]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
        {
            return Ok(await _tagRepository.GetAllTagsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            var tag = await _tagRepository.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Tag tag)
        {
            var createdTag = await _tagRepository.CreateTagAsync(tag);
            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, createdTag);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            await _tagRepository.DeleteTagAsync(id);
            return NoContent();
        }
    }
}