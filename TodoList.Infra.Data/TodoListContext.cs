using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;

namespace TodoList.Infra.Data
{
    public class TodoListContext : DbContext
    {        
        public TodoListContext(DbContextOptions options) : base(options)
        {            
        }

        public DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Notes>(entity => {
                entity.Property(x => x.Title)
                .HasMaxLength(500)
                .IsRequired();

                entity.Property(x => x.Content)
                .HasMaxLength(5000)
                .IsRequired();                
            });    

            // NotesMap.Configuration(modelBuilder);        
        }        
    }
}
