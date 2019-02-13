using AutoMapper;
using DataLayer;
using ToDoLib;

namespace ToDoApp
{
    public class AutoMapperMappings
    {
        public static void Initialize()
        {
            Mapper.CreateMap<ToDoItem, ToDoItemDto>().AfterMap((src, des) =>
            {
                des.HasNote = !string.IsNullOrWhiteSpace(src.ToDoItemNote?.Note);
            });

            Mapper.CreateMap<ToDoItemDto, ToDoItem>();
        }
    }
}