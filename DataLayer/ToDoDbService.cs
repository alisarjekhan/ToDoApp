using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess;
using ToDoLib;

namespace DataLayer
{
    public class ToDoDbService : IToDoRepoService
    {
        public ToDoDbService()
        {
            UpdateDatabase();
        }

        private static void UpdateDatabase()
        {
            using (var context = new FluentModelContext())
            {
                var schemaHandler = context.GetSchemaHandler();
                EnsureDB(schemaHandler);
            }
        }

        private static void EnsureDB(ISchemaHandler schemaHandler)
        {
            string script = null;
            if (schemaHandler.DatabaseExists())
            {
                script = schemaHandler.CreateUpdateDDLScript(null);
            }
            else
            {
                schemaHandler.CreateDatabase();
                script = schemaHandler.CreateDDLScript();
            }

            if (!string.IsNullOrEmpty(script))
            {
                schemaHandler.ExecuteDDLScript(script);
            }
        }

        public IList<ToDoItem> GetToDoItems()
        {
            using (var context = new FluentModelContext())
            {
                return context.ToDoItems.Where(t => !t.IsDeleted).ToList();
            }
        }

        public ToDoItem GetToDoItem(int id)
        {
            using (var context = new FluentModelContext())
            {
                return context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == id);
            }
        }

        public void AddToDoItem(ToDoItem toDoItem)
        {
            using (var context = new FluentModelContext())
            {
                toDoItem.Created = DateTime.Now;
                toDoItem.Updated = DateTime.Now;

                context.Add(toDoItem);
                context.SaveChanges();
            }
        }

        public bool UpdateToDoItem(ToDoItem toDoItem)
        {
            using (var context = new FluentModelContext())
            {
                return CheckAndUpdateToDoItem(toDoItem, context);
            }
        }

        public bool UpdateToDoItems(List<ToDoItem> toDoItems)
        {
            using (var context = new FluentModelContext())
            {
                bool isAnyToDoMissing = false;

                toDoItems.ForEach(toDoItem => 
                {
                    isAnyToDoMissing = CheckAndUpdateToDoItem(toDoItem, context);

                    //Update All OR None
                    if (isAnyToDoMissing)
                        return;
                });

                if (isAnyToDoMissing)
                    return false;

                context.SaveChanges();

                return true;
            }
        }

        private bool CheckAndUpdateToDoItem(ToDoItem toDoItem, FluentModelContext context)
        {
            var toDoItemFromDb = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItem.ToDoItemId);

            if (toDoItemFromDb == null || toDoItemFromDb.IsDeleted)
            {
                return false;
            }

            toDoItemFromDb.ToDoDescription = toDoItem.ToDoDescription;
            toDoItemFromDb.IsCompleted = toDoItem.IsCompleted;
            toDoItemFromDb.IsDeleted = toDoItem.IsDeleted;
            toDoItemFromDb.ToDoItemOrder = toDoItem.ToDoItemOrder;
            toDoItem.Updated = DateTime.Now;

            context.SaveChanges();

            return true;
        }

        public bool DeleteToDoItem(int toDoItemId)
        {
            using (var context = new FluentModelContext())
            {
                var toDoItemFromDb = context.ToDoItems.FirstOrDefault(t => t.ToDoItemId == toDoItemId);

                if (toDoItemFromDb == null || toDoItemFromDb.IsDeleted)
                {
                    return false;
                }

                toDoItemFromDb.Updated = DateTime.Now;
                toDoItemFromDb.IsDeleted = true;

                if (toDoItemFromDb.ToDoItemNote != null)
                    context.Delete(toDoItemFromDb.ToDoItemNote);

                context.SaveChanges();

                return true;
            }
        }

        public bool DeleteNoteForToDoItem(int toDoItemId)
        {
            using (var context = new FluentModelContext())
            {
                var toDoItemNoteFromDb = context.ToDoNotes.FirstOrDefault(t => t.ToDoItemId == toDoItemId);

                if (toDoItemNoteFromDb == null)
                {
                    return false;
                }

                context.Delete(toDoItemNoteFromDb);
                context.SaveChanges();

                return true;
            }
        }

        public void AddNoteForToDoItem(ToDoItem toDoItem, string note)
        {
            using (var context = new FluentModelContext())
            {
                var noteObj = new ToDoItemNote
                {
                    ToDoItemId = toDoItem.ToDoItemId,
                    Note = note
                };

                context.Add(noteObj);
                context.SaveChanges();
            }
        }

        public bool UpdateNoteToDoItem(ToDoItem toDoItem, string note)
        {
            using (var context = new FluentModelContext())
            {
                var noteFromDb = context.ToDoNotes.FirstOrDefault(n => n.ToDoItemId == toDoItem.ToDoItemId);
                if (noteFromDb == null)
                    return false;

                noteFromDb.Note = note;
                context.SaveChanges();

                return true;
            }
        }
    }
}
