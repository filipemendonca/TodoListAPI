using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoList.Infra.Data;

public class ApplicationDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TodoListContext>
{
    public TodoListContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TodoListContext>();
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ApplicationDb;integrated security=true;");

        return new TodoListContext(optionsBuilder.Options);
    }
}