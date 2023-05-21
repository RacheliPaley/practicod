using Microsoft.OpenApi.Models;
using ToDoAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("OpenPolicy",
//                           policy =>
//                           {
//                               policy.WithOrigins("https://authclient-o1zx.onrender.com/")
//                                                   .AllowAnyHeader()
//                                                   .AllowAnyMethod();
//                           });
// });
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Description = "Keep track of your tasks", Version = "v1" });
// });

// builder.Services.AddDbContext<ToDoDbContext>();

//    builder.Services.AddCors(options =>
//     {
//         options.AddPolicy("AllowRenderClient",
//             builder =>
//             {
//                 builder.WithOrigins("https://authclient-o1zx.onrender.com")
//                     .AllowAnyHeader()
//                     .AllowAnyMethod();
//             });
//     });

//     // Other services here

// var app = builder.Build();
// app.UseCors("AllowRenderClient");
// app.UseCors("OpenPolicy");
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseSwagger(options => { options.SerializeAsV2 = true; });
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
//     c.RoutePrefix = string.Empty;
// });
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddSwaggerGen();

var app = builder.Build();
Console.WriteLine();

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
// app.MapGet("/", () => "Hello World!");

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