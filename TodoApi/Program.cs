using Microsoft.OpenApi.Models;
using ToDoAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

 var policyName = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("*")
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});
// builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddDbContext<ToDoDbContext>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
    });
});
var app = builder.Build();

app.UseCors(policyName);
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(options =>
{
options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
options.RoutePrefix = string.Empty;
});
// }
app.MapGet("/todoitems", async (ToDoDbContext db) =>

 await db.Items.ToListAsync());


app.MapPost("/todoitems", async ([FromBody] Item todo, ToDoDbContext db) =>
{
    db.Items.Add(todo);
    await db.SaveChangesAsync();
       
    return todo;//Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, [FromBody] Item inputTodo, ToDoDbContext db) =>
{
    var todo = await db.Items.FindAsync(id);
    if (todo is null) return Results.NotFound();
    //todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/todoitems/{id}", async (int id, ToDoDbContext db) =>
{
    var existItem = await db.Items.FindAsync(id);
    if (existItem is null) return Results.NotFound();
    db.Items.Remove(existItem);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/",()=>"AuthServer API is running");
app.Run();