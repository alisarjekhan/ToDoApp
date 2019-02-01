namespace ToDoLib
{
    public interface IToDoItemDto
    {
        int ToDoItemId { get; set; }
        string ToDoDescription { get; set; }
        //int NoteCount { get; }
        bool IsCompleted { get; set; }
    }
}
