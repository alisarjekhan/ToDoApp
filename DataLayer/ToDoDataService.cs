//using System.Collections.Generic;
//using System.Linq;
//using ToDoLib;

//namespace DataLayer
//{
//    public class ToDoDataService : IToDoRepoService
//    {
//        static Dictionary<int, IToDoItem> _dataStore = new Dictionary<int, IToDoItem>
//                                    {
//                                        {
//                                            1,
//                                            new ToDoItem
//                                            {
//                                                ToDoItemId = 1,
//                                                ToDoDescription = "Test01",
//                                                IsCompleted = false,
//                                                IsDeleted = false
//                                            }
//                                        },

//                                        {
//                                            2,
//                                            new ToDoItem
//                                            {
//                                                ToDoItemId = 2,
//                                                ToDoDescription = "Test02",
//                                                IsCompleted =false,
//                                                IsDeleted = false
//                                            }
//                                        },

//                                        {
//                                            3,
//                                            new ToDoItem
//                                            {
//                                                ToDoItemId = 3,
//                                                ToDoDescription = "Test03",
//                                                IsCompleted =false,
//                                                IsDeleted = false
//                                            }
//                                        }
//                                    };

//        public IList<IToDoItem> GetToDoItems()
//        {
//            return _dataStore.Values.ToList();
//        }

//        public IToDoItem GetToDoItem(int toDoItemId)
//        {
//            return _dataStore[toDoItemId];
//        }

//        public void UpdateToDoItem(IToDoItem toDoItem)
//        {
//            _dataStore[toDoItem.ToDoItemId] = toDoItem;
//        }

//        public bool DeleteToDoItem(int toDoItemId)
//        {
//            return _dataStore.Remove(toDoItemId);
//        }

//        public void AddToDoItem(IToDoItem toDoItem)
//        {
//            _dataStore.Add(toDoItem.ToDoItemId, toDoItem);
//        }
//    }
//}
