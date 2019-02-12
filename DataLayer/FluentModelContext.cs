using System;
using System.Linq;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using ToDoLib;

namespace DataLayer
{
    internal class FluentModelContext : OpenAccessContext
    {
        private static string connectionStringName = @"TodoAppConnection";

        private static BackendConfiguration backend =
            GetBackendConfiguration();

        private static MetadataSource metadataSource =
            new FluentModelMetadataSource();

        public FluentModelContext()
            : base(connectionStringName, backend, metadataSource)
        { }

        public IQueryable<ToDoItem> ToDoItems
        {
            get
            {
                return this.GetAll<ToDoItem>();
            }
        }

        public IQueryable<ToDoItemNote> ToDoNotes
        {
            get
            {
                return this.GetAll<ToDoItemNote>();
            }
        }

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration fromCodeConfiguration = new BackendConfiguration()
            {
                Backend = "MsSql",
                ProviderName = "System.Data.SqlClient"
            };

            BackendConfiguration.MergeBackendConfigurationFromConfigFile(fromCodeConfiguration,
                ConfigurationMergeMode.ConfigFileDefinitionWins, "AzureConfiguration");
            //FluentModel dbContext = new FluentModel(fromCodeConfiguration);
            //BackendConfiguration backend = new BackendConfiguration();
            //{
            //Backend = "MsSql",
            //ProviderName = "System.Data.SqlClient"
            //}

            //BackendConfiguration backend = new BackendConfiguration();
            //backend.Backend = "Azure";
            //backend.ProviderName = "System.Data.SqlClient";

            //CustomizeBackendConfiguration(ref backend);

            return fromCodeConfiguration;
        

            //return backend;
        }

        static void CustomizeBackendConfiguration(ref BackendConfiguration config)
        {
            config.Runtime.AllowReadAfterDelete = true;
            config.SecondLevelCache.Enabled = true;
            config.SecondLevelCache.NumberOfObjects = 6000;
        }
    }
}