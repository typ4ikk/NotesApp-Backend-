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
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        public NotesController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return Ok(await _noteRepository.GetAllNotesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            var createdNote = await _noteRepository.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            try
            {
                await _noteRepository.UpdateNoteAsync(note);
            }
            catch
            {
                if (await _noteRepository.GetNoteByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            await _noteRepository.DeleteNoteAsync(id);
            return NoContent();
        }

        [HttpGet("with-reminders")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesWithReminders()
        {
            return Ok(await _noteRepository.GetNotesWithRemindersAsync());
        }

        [HttpGet("by-tag/{tagId}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByTag(int tagId)
        {
            return Ok(await _noteRepository.GetNotesByTagAsync(tagId));
        }
    }
}