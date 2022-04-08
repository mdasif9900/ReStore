using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Create Kestrel server with some default settings
           var host =  CreateHostBuilder(args).Build();
           using var scope = host.Services.CreateScope();
           var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        //    to display the error in command
           var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
           try{
               context.Database.Migrate();
               DbInitializer.Initialize(context);
           }
           catch(Exception ex)
           {
               logger.LogError(ex,"problem migrating data");
           }
          host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
