using System.Collections.Generic;

namespace ToDoLib
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
        void UpdateToDoItem(ToDoItem toDoItem);
        bool DeleteToDoItem(int toDoItemId);
    }
}
