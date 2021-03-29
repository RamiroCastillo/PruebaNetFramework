using Microsoft.Extensions.DependencyInjection;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.DataAccess;
using Test.DataAccess.V1;

namespace WebApplicationNetFramework
{
    public partial class Startup
    {
        public static void Configuration()
        {
            var services = new ServiceCollection();
            //ConfigureAuth(app);
            ConfigureServices(services);
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
   .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
   .Where(t => typeof(IController).IsAssignableFrom(t)
      || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            #region Config DataBase

            string dbApiDatabase = ConfigurationManager.AppSettings["Database"];
            string dbApiPassword = ConfigurationManager.AppSettings["Password"];
            string dbApiServer = ConfigurationManager.AppSettings["Server"];
            string dbApiUser = ConfigurationManager.AppSettings["User"];
            int dbApiTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
            //dbApiPassword = securityManager.EncryptDecrypt(false, dbApiPassword); ENCRIPTAR
            string connectionDbApi = Connection.DBApi(dbApiServer, dbApiDatabase, dbApiUser, dbApiPassword, "WebApiPrueba");
            #endregion

            services.AddSingleton<IProduct, Product>(f => new Product(connectionDbApi, dbApiTimeout));
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
           IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}