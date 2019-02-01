using System.Runtime.Serialization;

namespace ToDoLib
{
    public class ToDoItemDto //: IToDoItemDto
    {
        public int ToDoItemId { get; set; }
        public string ToDoDescription { get; set; }
        public bool IsCompleted { get; set; }
        public int ToDoItemOrder { get; set; }

        //public bool HasNotes => NoteCount > 0;
        //public string Note { get; set; }

        public bool HasNote { get; set; }
    }
}