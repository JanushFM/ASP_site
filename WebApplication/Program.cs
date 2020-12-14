using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;


namespace WebApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await DataInitializer.InitializeAsync(rolesManager);
            }
            catch (Exception)
            {
                Debug.WriteLine("Something goes wrong with data seeding");
            }
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var settings = config.Build();

                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(settings["ConnectionStrings:AppConfig"])
                                    .ConfigureKeyVault(kv =>
                                    {
                                        kv.SetCredential(new DefaultAzureCredential());
                                    });
                            });
                        })
                        .ConfigureLogging((hostingContext, logging) =>
                        {
                            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                            logging.AddConsole();
                            logging.AddDebug();
                            logging.AddEventSourceLogger();
                            logging.AddNLog();
                        })
                        .UseStartup<Startup>());
    }
}