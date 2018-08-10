using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC.Core.Data;
using POC.Data;
using POC.Service;

namespace POC.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //SQL Server Express LocalDB 
            var defaultConnection = "Server = (localdb)\\mssqllocaldb; Database = ContosoUniversity1; Trusted_Connection = True; MultipleActiveResultSets = true";

            ////Register the context with dependency injection
            //services.AddDbContext<SchoolContext>(options => options.UseSqlServer(defaultConnection) );








            #region Imp DI configurations

            //Register the context with dependency injection
            services.AddDbContext<PocDbContext>(options => options.UseSqlServer(defaultConnection));

            //services.AddDbContext<ExampleContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            //PocDbContext
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IStudentService, StudentService>();


            #endregion





            //services.AddDbContext<SchoolContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<PocDbContext>(options =>
            //options.UseSqlServer(Configuration[@"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;"]));

            //.UseSqlServer(Configuration["database:connectionString"]));


            //services.AddTransient<IUserService, UserService>();

            ////services.AddScoped(typeof(IRepository<User>), typeof(GenericRepository<,>));
            //services.AddScoped<DbContext, PocDbContext>();


            //services.AddScoped<IDbContext, PocDbContext>();
            //services.AddScoped<IUserService, UserService>();
            ////services.AddTransient<IDbContext, PocDbContext>();
            ////services.AddTransient<IUserService, UserService>();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
