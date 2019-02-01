using ToDoLib;

namespace DataLayer
{
    public class ToDoItem //: IToDoItem
    {
        public int ToDoItemId { get; set; }
        public string ToDoDescription { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public int ToDoItemOrder { get; set; }

        public ToDoItemNote ToDoItemNote { get; set; }
    }
}
