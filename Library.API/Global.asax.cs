using Library.API.App_Start;
using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories;
using Library.Infrastructure.Utilities;
using Library.Service.Interfaces;
using Library.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                //services.AddHttpClient("LoggingAPI", httpClient =>
                //{
                //    httpClient.BaseAddress = new Uri("https://logging.library.com/api/");
                //    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //})
                //.ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler
                //{
                //    Credentials = new CredentialCache
                //        {
                //            {new Uri ("https://logging.library.com/api/"), "NTLM", new NetworkCredential("username", "password", "domain") }
                //        }
                //});

                //services.AddTransient<IBookRepository, BookRepository>();
                //services.AddTransient<IBookService, BookService>(s =>
                //{
                //    return new BookService(s.GetRequiredService<IBookRepository>(), connectionString);
                //});

                services.AddTransient<IExternalAPIUtility, ExternalAPIUtility>(u =>
               {
                   var client = new HttpClient(new HttpClientHandler
                   {
                       Credentials = new CredentialCache
                       {
                            {new Uri ("https://logging.library.com/api/"), "NTLM", new NetworkCredential("username", "password", "domain") }
                       }
                   });
                   client.BaseAddress = new Uri("https://logging.library.com/api/");
                   client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                   return new ExternalAPIUtility(client);
               });
                services.AddTransient<ILogService, LogService>(l =>
                {
                    return new LogService(l.GetRequiredService<IExternalAPIUtility>());
                });
            });
        }
    }
}
