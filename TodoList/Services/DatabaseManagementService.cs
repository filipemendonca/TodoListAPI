using Microsoft.EntityFrameworkCore;
using TodoList.Infra.Data;

public static class DatabaseManagementService {
    public static void ConfigureMigrations(IApplicationBuilder app) {
        using (var serviceScope = app.ApplicationServices.CreateScope()){
            serviceScope.ServiceProvider.GetService<TodoListContext>().Database.Migrate();
        }
    }
}