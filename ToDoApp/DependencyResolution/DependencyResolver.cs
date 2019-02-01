using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace ToDoApp.DependencyResolution
{
    public class DependencyResolver : IDependencyResolver
    {
        IContainer _container;

        public DependencyResolver(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException("container");
        }

        public IDependencyScope BeginScope()
        {
            IContainer child = _container.GetNestedContainer();
            return new DependencyResolver(child);
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            if (serviceType.IsAbstract || serviceType.IsInterface)
            {
                return _container.TryGetInstance(serviceType);
            }

            return _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}