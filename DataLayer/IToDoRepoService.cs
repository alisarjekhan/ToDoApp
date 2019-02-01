using System.Collections.Generic;

namespace DataLayer
{
    //public interface IToDoRepoService
    //{
    //    IList<IToDoItem> GetToDoItems();
    //    IToDoItem GetToDoItem(int id);
    //    void AddToDoItem(IToDoItem toDoItem);
    //    void UpdateToDoItem(IToDoItem toDoItem);
    //    bool DeleteToDoItem(int toDoItemId);
    //}

    public interface IToDoRepoService
    {
        IList<ToDoItem> GetToDoItems();
        ToDoItem GetToDoItem(int id);
        void AddToDoItem(ToDoItem toDoItem);
        bool UpdateToDoItem(ToDoItem toDoItem);
        bool UpdateToDoItems(List<ToDoItem> toDoItems);
        bool DeleteToDoItem(int toDoItemId);

        bool DeleteNoteForToDoItem(int toDoItemId);
        void AddNoteForToDoItem(ToDoItem toDoItem, string note);
        bool UpdateNoteToDoItem(ToDoItem toDoItem, string note);
        
    }
}
