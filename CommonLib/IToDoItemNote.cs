namespace ToDoLib
{
    public interface IToDoItemNote
    {
        int ToDoItemId { get; set; }
        string Note { get; set; }

        IToDoItem ToDoItem { get; set; }
    }
}