using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Contexts;
using Persistence.Repositories;
using WebApplication.EmailSender;

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
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString(nameof(ApplicationContext)),
                    b => b.MigrationsAssembly("WebApplication")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            

            services.AddAuthentication().
                AddGoogle(options =>
                {
                    options.ClientId = Configuration["GoogleClientId"];
                    options.ClientSecret = Configuration["GoogleClientSecret"];
                });
            services.AddScoped(typeof(IMovieRepository), typeof(MovieRepository));
            services.AddScoped(typeof(IArtistRepository), typeof(ArtistRepository));
            services.AddScoped(typeof(IDescriptionRepository), typeof(DescriptionRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IPaintingRepository), typeof(PaintingRepository));
            services.AddTransient<IMailSender, EmailSender.EmailSender>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }    
            else if (env.IsProduction())
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", 
                        optional: false, 
                        reloadOnChange: true)
                    .AddEnvironmentVariables();

                if (env.IsDevelopment())
                {
                    builder.AddUserSecrets<Startup>();
                }

                builder.AddUserSecrets<Startup>();
                
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                
            }
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            SeedRoles.InitRoles(roleManager);
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Artists}/{action=Index}/{id?}");
            });
        }
    }
}