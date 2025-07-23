using System.ComponentModel.DataAnnotations;

namespace NotesApp.Server.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? ReminderDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public bool isReminder { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }
}
