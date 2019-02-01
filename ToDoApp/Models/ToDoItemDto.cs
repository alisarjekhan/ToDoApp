using ToDoLib;

namespace ToDoApp.Models
{
    public class ToDoItemDto : IToDoItemDto
    {
        public int ToDoId { get; set; }
        public string ToDoDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}