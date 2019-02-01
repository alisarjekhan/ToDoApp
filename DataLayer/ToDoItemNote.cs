using ToDoLib;

namespace DataLayer
{
    public class ToDoItemNote//:IToDoItemNote
    {
        public int ToDoItemId { get; set; }
        public string Note { get; set; }

        //IToDoItem _toDoItem;
        public ToDoItem ToDoItem { get; set; }// { return _toDoItem; } set { _toDoItem = value; } }
    }
}