using Microsoft.AspNetCore.Mvc;
using NotesApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesApp.Server.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<Note?> GetNoteByIdAsync(int id);
        Task<Note> CreateNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(int id);
        Task<ActionResult<IEnumerable<Note>>> GetNotesWithRemindersAsync();
        Task<IEnumerable<Note>> GetNotesByTagAsync(int tagId); 
    }
}