using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesApp.Server.Data;
using NotesApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace NotesApp.Server.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationContext _context;

        public NoteRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _context.Notes
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await _context.Notes
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task UpdateNoteAsync(Note note)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));

            var existingNote = await _context.Notes
                .Include(n => n.NoteTags)
                .FirstOrDefaultAsync(n => n.Id == note.Id);

            if (existingNote == null)
                throw new KeyNotFoundException($"Note with id {note.Id} not found");

            _context.Entry(existingNote).CurrentValues.SetValues(note);
            existingNote.NoteTags.Clear();

            foreach (var noteTag in note.NoteTags)
            {
                existingNote.NoteTags.Add(noteTag);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _context.Notes
                .Include(n => n.NoteTags)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note != null)
            {
                _context.NoteTags.RemoveRange(note.NoteTags);
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ActionResult<IEnumerable<Note>>> GetNotesWithRemindersAsync()
        {
            return await _context.Notes
                .Where(n => n.ReminderDate != null)
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetNotesByTagAsync(int tagId)
        {
            return await _context.Notes
                .Where(n => n.NoteTags.Any(nt => nt.TagId == tagId))
                .Include(n => n.NoteTags)   
                .ThenInclude(nt => nt.Tag)
                .ToListAsync();
        }
    }
}