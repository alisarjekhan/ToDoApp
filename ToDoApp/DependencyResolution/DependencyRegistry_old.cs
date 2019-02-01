using AutoMapper;
using DataLayer;
using StructureMap.Configuration.DSL;
using ToDoLib;

namespace ToDoApp.DependencyResolution
{
    public class DependencyRegistry : Registry {
        #region Constructors and Destructors

        public DependencyRegistry() {
            //Scan(
            //    scan => {
            //        scan.TheCallingAssembly();
            //        scan.WithDefaultConventions();
            //    });


            //Mapper.AddProfile<ToDoItemProfile>();

            //Mapper.CreateMap<IToDoItem, ToDoItemDto>();
            //Mapper.CreateMap<ToDoItemDto, ToDoItem>();

            Mapper.CreateMap<ToDoItem, ToDoItemDto>().ForMember(t => t.Note, opt => opt.MapFrom(x => x.ToDoItemNote.Note));

            Mapper.CreateMap<ToDoItemDto, ToDoItem>().AfterMap((src, des) =>
                                                                {
                                                                    if (des.ToDoItemNote == null)
                                                                    {
                                                                        des.ToDoItemNote = new ToDoItemNote();

                                                                    }

                                                                    des.ToDoItemNote.ToDoItemId = src.ToDoItemId;
                                                                    des.ToDoItemNote.Note = src.Note;
                                                                    des.ToDoItemNote.ToDoItem = des;
                                                                });


            //Mapper.CreateMap<ToDoItem, ToDoItemDto>().AfterMap((src, des) =>
            //{ 
            //    des.Note = src.ToDoItemNote?.Note;
            //});

            //Mapper.CreateMap<ToDoItemDto, ToDoItem>().AfterMap((src, des) =>
            //{
            //    //if (src.Note != null)
            //    //{ 
            //    //    des.ToDoItemNote = new ToDoItemNote();
            //    //}

            //    //des.ToDoItemNote.ToDoItemId = src.ToDoItemId;
            //    //des.ToDoItemNote.Note = src.Note;
            //    //des.ToDoItemNote.ToDoItem = des;
            //});

            For<IToDoRepoService>().Use<ToDoDbService>();
        }

        #endregion
    }
}