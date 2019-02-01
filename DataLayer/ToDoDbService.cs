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

        public IList<ToDoItem> GetToDoItems()
        {
            using (var context = new FluentModelContext())
            {
                return context.ToDoItems.Where(t => !t.IsDeleted).ToList();
            }
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
                context.Add(toDoItem);
                context.SaveChanges();
            }
        }

        public bool UpdateToDoItem(ToDoItem toDoItem)
        {
            using (var context = new FluentModelContext())
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

                //if (toDoItem.ToDoItemNote != null)
                //{
                //    toDoItemFromDb.ToDoItemNote.ToDoItemId = toDoItem.ToDoItemNote.ToDoItemId;
                //    toDoItemFromDb.ToDoItemNote.Note = toDoItem.ToDoItemNote.Note;
                //}
                context.SaveChanges();

                return true;
            }
        }

        public bool UpdateToDoItems(List<ToDoItem> toDoItems)
        {
            using (var context = new FluentModelContext())
            {
                var isAnyToDoMissing = false;
                toDoItems.ForEach(toDoItem => 
                {
                    var toDoItemFromDb = context.ToDoItems.FirstOrDefault(t => toDoItem.ToDoItemId == t.ToDoItemId);
                    if (toDoItemFromDb == null || toDoItemFromDb.IsDeleted)
                    {
                        isAnyToDoMissing = true;
                        return;
                    }

                    toDoItemFromDb.ToDoDescription = toDoItem.ToDoDescription;
                    toDoItemFromDb.IsCompleted = toDoItem.IsCompleted;
                    toDoItemFromDb.IsDeleted = toDoItem.IsDeleted;
                    toDoItemFromDb.ToDoItemOrder = toDoItem.ToDoItemOrder;

                    //if (toDoItem.ToDoItemNote != null)
                    //{
                    //    toDoItemFromDb.ToDoItemNote.ToDoItemId = toDoItem.ToDoItemNote.ToDoItemId;
                    //    toDoItemFromDb.ToDoItemNote.Note = toDoItem.ToDoItemNote.Note;
                    //}
                });

                if (isAnyToDoMissing)
                    return false;

                context.SaveChanges();

                return true;
            }
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
