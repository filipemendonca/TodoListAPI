using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;

public static class NotesMap {
    public static void Configuration(ModelBuilder modelBuilder){
        modelBuilder.Entity<Notes>(entity => {
            entity.Property(x => x.Title)
            .HasMaxLength(500)
            .IsRequired();
            
            entity.Property(x => x.Content)
            .HasMaxLength(5000)
            .IsRequired();                
        });    
    }
}