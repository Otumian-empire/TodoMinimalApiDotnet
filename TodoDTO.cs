public class TodoDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }

    public TodoDTO() { }

    public TodoDTO(TodoModel item)
    {
        (Id, Name, IsComplete) = (item.Id, item.Name, item.IsComplete);
    }
}