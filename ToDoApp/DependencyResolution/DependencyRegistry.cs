using AutoMapper;
using DataLayer;
using StructureMap.Configuration.DSL;
using ToDoLib;

namespace ToDoApp.DependencyResolution
{
    public class DependencyRegistry : Registry {
        #region Constructors and Destructors

        public DependencyRegistry()
        {
            Mapper.CreateMap<ToDoItem, ToDoItemDto>().AfterMap((src, des) =>
            {
                des.HasNote = !string.IsNullOrWhiteSpace(src.ToDoItemNote?.Note);
            });

            Mapper.CreateMap<ToDoItemDto, ToDoItem>();

            For<IToDoRepoService>().Use<ToDoDbService>();
        }

        #endregion
    }
}