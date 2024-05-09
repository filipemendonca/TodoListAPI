using Microsoft.EntityFrameworkCore;
using TodoList.Infra.Data;
using TodoList.Infra.Data.Repository;
using TodoList.Infra.Data.Repository.Interface;
using TodoList.MapperProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoListContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoListDb"));
});

//Add scope of Repositories
builder.Services.AddScoped<INotesRepository, NotesRepository>();

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(NotesProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
