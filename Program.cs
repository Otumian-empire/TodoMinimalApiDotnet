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
route.MapPost("/", CreateTodoItem);

// Read all todo items
route.MapGet("", GetAllTodoItems);

// Read all completed todo items
route.MapGet("/complete", ReadAllCompletedTodItems);

// Read todo item by id
route.MapGet("/{id}", ReadTodoItemById);

// Update todo item by id
route.MapPut("/{id}", UpdateTodoItemById);

// Delete todo item by id
route.MapDelete("/{id}", DeleteTodoItemById);

app.Run();


static async Task<IResult> CreateTodoItem(TodoModel todo, TodoDb db)
{
    db.TodoTable.Add(todo);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/{todo.Id}", todo);
}

static async Task<IResult> GetAllTodoItems(TodoDb db)
{
    var result = await db.TodoTable.ToListAsync();

    return TypedResults.Ok(result);
}

static async Task<IResult> ReadAllCompletedTodItems(TodoDb db)
{
    var result = await db.TodoTable
            .Where(row => row.IsComplete)
            .ToListAsync();

    return TypedResults.Ok(result);
}

static async Task<IResult> ReadTodoItemById(int id, TodoDb db)
{
    var todo = await db.TodoTable.FindAsync(id);

    if (todo is not null)
        return TypedResults.Ok(todo);

    return TypedResults.NotFound();
}

static async Task<IResult> UpdateTodoItemById(int id, TodoModel newTodo, TodoDb db)
{
    var todo = await db.TodoTable.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = newTodo.Name;
    todo.IsComplete = newTodo.IsComplete;

    await db.SaveChangesAsync();

    // return TypedResults.NoContent();
    return TypedResults.Ok(todo);
}

static async Task<IResult> DeleteTodoItemById(int id, TodoDb db)
{
    var todo = await db.TodoTable.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    db.TodoTable.Remove(todo);
    await db.SaveChangesAsync();

    return TypedResults.Ok(todo);
}
