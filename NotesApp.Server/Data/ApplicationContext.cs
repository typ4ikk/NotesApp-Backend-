using NotesApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace NotesApp.Server.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteTag>()
                .HasKey(nt => new { nt.NoteId, nt.TagId });
            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId);
            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId);
        }
    }
}
