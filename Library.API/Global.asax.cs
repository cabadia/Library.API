using Library.API.App_Start;
using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories;
using Library.Service.Interfaces;
using Library.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Library.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

            GlobalConfiguration.Configuration.AddMicrosoftDependencyInjectionProvider(services =>
            {
                services.AddControllersAsServices(System.Reflection.Assembly.GetExecutingAssembly());
                services.AddTransient<IBookRepository, BookRepository>();
                services.AddTransient<IBookService, BookService>(s =>
                {
                    return new BookService(s.GetRequiredService<IBookRepository>(), connectionString);
                });
            });
        }
    }
}
