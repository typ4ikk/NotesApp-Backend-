﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Server.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>(); 
    }
}
