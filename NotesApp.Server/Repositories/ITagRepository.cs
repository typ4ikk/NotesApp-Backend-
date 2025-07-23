using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Server.Models;

namespace NotesApp.Server.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
    }
}