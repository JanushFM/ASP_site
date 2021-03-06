using Application.Interfaces.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Contexts;
using Persistence.Repositories;
using Services.Interfaces;
using Services.Services;
using WebApplication.Hubs;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(nameof(ApplicationContext)),
                    b => b.MigrationsAssembly("WebApplication")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            
            services.Configure<AzureStorageConfig>(Configuration.GetSection(nameof(AzureStorageConfig)));
            
            services.AddAuthentication().
                AddGoogle(options =>
                {
                    options.ClientId = Configuration["Settings:GoogleClientId"];
                    options.ClientSecret = Configuration["Settings:GoogleClientSecret"];
                });
            
            services.AddTransient(typeof(IArtistRepository), typeof(ArtistRepository));
            services.AddTransient(typeof(IDescriptionRepository), typeof(DescriptionRepository));
            services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient(typeof(IPaintingRepository), typeof(PaintingRepository));
            services.AddSingleton<IMailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }    
            else if (env.IsProduction())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Artists}/{action=Index}/{id?}");
                endpoints.MapHub<OrderHub>("/chathub");
            });
        }
    }
}