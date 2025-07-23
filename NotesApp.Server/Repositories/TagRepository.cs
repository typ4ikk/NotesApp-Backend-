using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApp.Server.Models;
using NotesApp.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.Server.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationContext _context;

        public TagRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags
                .Include(t => t.NoteTags)
                .ThenInclude(nt => nt.Note)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.NoteTags)
                .ThenInclude(nt => nt.Note)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _context.Tags
                .Include(t => t.NoteTags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tag != null)
            {
                _context.NoteTags.RemoveRange(tag.NoteTags);
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}