using StructureMap;
using System.Web.Http;
using ToDoApp.DependencyResolution;

namespace ToDoApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var container = IoC.Initialize();
            //config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            var container = new Container(c => c.AddRegistry<DependencyRegistry>());
            config.DependencyResolver = new DependencyResolver(container); 

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{toDoItemId}",
                defaults: new { toDoItemId = RouteParameter.Optional }
            );
        }
    }
}
