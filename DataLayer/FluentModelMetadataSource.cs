using System.Collections.Generic;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Metadata.Fluent;
using ToDoLib;

namespace DataLayer
{
    internal class FluentModelMetadataSource : FluentMetadataSource
    {
        protected override IList<MappingConfiguration> PrepareMapping()
        {
            List<MappingConfiguration> configurations = new List<MappingConfiguration>();

            MappingConfiguration<ToDoItem> toDoItemConfiguration = new MappingConfiguration<ToDoItem>();
            toDoItemConfiguration.MapType(t => new
            {
                t.ToDoItemId,
                t.ToDoDescription,
                t.IsCompleted,
                t.ToDoItemOrder,
                t.Created,
                t.Updated,
                t.IsDeleted
            }).ToTable("ToDoItem");
            toDoItemConfiguration.HasProperty(p => p.ToDoItemId).IsIdentity(KeyGenerator.Autoinc);

            MappingConfiguration<ToDoItemNote> toDoItemNoteConfiguration = new MappingConfiguration<ToDoItemNote>();
            toDoItemNoteConfiguration.MapType(c => new
            {
                c.ToDoItemId,
                c.Note
            }).ToTable("ToDoItemNote");
            toDoItemNoteConfiguration.HasProperty(p => p.ToDoItemId).IsIdentity();

            toDoItemNoteConfiguration
                .HasAssociation(x => x.ToDoItem)
                .WithOpposite(o => o.ToDoItemNote)
                .HasConstraint((x, o) => x.ToDoItemId == o.ToDoItemId)
                .IsRequired();

            configurations.Add(toDoItemConfiguration);
            configurations.Add(toDoItemNoteConfiguration);

            return configurations;
        }
    }
}