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
            AutoMapperMappings.Initialize();

            For<IToDoRepoService>().Use<ToDoDbService>();
        }

        #endregion
    }
}