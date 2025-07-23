using System;

namespace NotesApp.Server.Models
{
    public class NoteTag
    {
        public int NoteId { get; set; }
        public Note Note { get; set; } = new Note();

        public int TagId {get; set; }
        public Tag Tag { get; set; } = new Tag();
    }
}
