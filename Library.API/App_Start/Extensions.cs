using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Library.API.App_Start
{
    public static class HttpConfigurationExtensions
    {
        public static IServiceProvider AddMicrosoftDependencyInjectionProvider(
            this HttpConfiguration config,
            Action<IServiceCollection> configure = null
        )
        {
            var provider = IServiceCollectionExtensions.BuildServiceProvider(configure);

            var resolver = new DefaultDependencyResolver(provider);

            config.DependencyResolver = resolver;

            return provider;
        }
    }

    public static class HttpApplicationExtensions
    {
        public static IServiceProvider BuildServiceProvider(this HttpApplication application, Action<IServiceCollection> configure = null)
        {
            var provider = IServiceCollectionExtensions.BuildServiceProvider(configure);
            return provider;
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure = null)
        {
            var services = new ServiceCollection();

            configure?.Invoke(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
            Assembly controllersAssembly)
        {
            var controllerTypes = GetControllers(controllersAssembly);

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }

        public static IEnumerable<Type> GetControllers(Assembly controllersAssembly)
        {
            var controllers = controllersAssembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));

            return controllers;
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider ServiceProvider { get; set; }

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this.ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.ServiceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(this.ServiceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            // you can implement this interface just when you use .net core 2.0

            // this.ServiceProvider.Dispose();
        }

    }

}