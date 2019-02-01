namespace ToDoLib
{
    public interface IToDoItem //: IToDoItemDto
    {
        int ToDoItemId { get; set; }
        string ToDoDescription { get; set; }
        //int NoteCount { get; }
        bool IsCompleted { get; set; }

        bool IsDeleted { get; }

        IToDoItemNote ToDoItemNote { get; set; }
    }
}
