using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database connection
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// WebApplication
var app = builder.Build();

app.MapGet("/", () => "Hello world!");

// The Todo API starts here
// Use MapGroup to group endpoints of the same base
var route = app.MapGroup("/todoitems");

// Create todo item
route.MapPost("/", Controller.CreateTodoItem);

// Read all todo items
route.MapGet("/", Controller.GetAllTodoItems);

// Read all completed todo items
route.MapGet("/complete", Controller.ReadAllCompletedTodItems);

// Read todo item by id
route.MapGet("/{id}", Controller.ReadTodoItemById);

// Update todo item by id
route.MapPut("/{id}", Controller.UpdateTodoItemById);

// Delete todo item by id
route.MapDelete("/{id}", Controller.DeleteTodoItemById);

app.Run();
