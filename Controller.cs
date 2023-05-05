using Microsoft.EntityFrameworkCore;

public class Controller
{
    internal static async Task<IResult> CreateTodoItem(TodoModel todo, TodoDb db)
    {
        db.TodoTable.Add(todo);
        await db.SaveChangesAsync();

        /* return TypedResults.Created($"/{todo.Id}", todo); */
        return TypedResults.Created($"/{todo.Id}", new TodoDTO(todo));

    }

    internal static async Task<IResult> GetAllTodoItems(TodoDb db)
    {
        /* var result = await db.TodoTable.ToListAsync(); */
        var result = await db.TodoTable.Select((x => new TodoDTO(x))).ToArrayAsync();

        return TypedResults.Ok(result);
    }

    internal static async Task<IResult> ReadAllCompletedTodItems(TodoDb db)
    {
        /* var result = await db.TodoTable
                .Where(row => row.IsComplete)
                .ToListAsync(); */

        var result = await db.TodoTable
            .Where(row => row.IsComplete)
            .Select(x => new TodoDTO(x))
            .ToArrayAsync();

        return TypedResults.Ok(result);
    }

    internal static async Task<IResult> ReadTodoItemById(int id, TodoDb db)
    {
        var todo = await db.TodoTable.FindAsync(id);

        if (todo is not null)
            /* return TypedResults.Ok(todo); */
            return TypedResults.Ok(new TodoDTO(todo));

        return TypedResults.NotFound();
    }

    internal static async Task<IResult> UpdateTodoItemById(int id, TodoModel newTodo, TodoDb db)
    {
        var todo = await db.TodoTable.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = newTodo.Name;
        todo.IsComplete = newTodo.IsComplete;

        await db.SaveChangesAsync();

        // return TypedResults.NoContent();
        return TypedResults.Ok(new TodoDTO(todo));
    }

    internal static async Task<IResult> DeleteTodoItemById(int id, TodoDb db)
    {
        var todo = await db.TodoTable.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        db.TodoTable.Remove(todo);
        await db.SaveChangesAsync();

        /* return TypedResults.Ok(todo); */
        return TypedResults.Ok(new TodoDTO(todo));
    }


}
