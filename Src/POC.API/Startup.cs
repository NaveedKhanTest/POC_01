

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Morcatko.AspNetCore.JsonMergePatch;
using POC.API.Filters;
using POC.Data;
using POC.Service;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;




namespace POC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // https://dotnetcore.gaprogman.com/2017/12/07/giving-dwcheckapi-that-swagger/ 
            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore

            services.AddMvc().AddJsonMergePatch();

            //services.AddMvc()
            //        .AddJsonMergePatch()
            //        .AddJsonOptions(options =>
            //        {
            //            //don't use using System.Xml;
            //            options.SerializerSettings.Formatting = Formatting.Indented;
            //        });

            #region DI configurations

            //Register the context with dependency injection
            var defaultConnection = "Server = (localdb)\\mssqllocaldb; Database = ContosoUniversity1; Trusted_Connection = True; MultipleActiveResultSets = true";
            var connectionStr = this.Configuration.GetConnectionString("PocDb");
            services.AddDbContext<PocDbContext>(options => options.UseSqlServer(defaultConnection));
            //services.AddDbContext<PocDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("PocDb")));
            //services.AddDbContext<ExampleContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IStudentService, StudentService>();

            if (HostingEnvironment.IsDevelopment())
            {
                //services.AddScoped<ISomeHandler, SomeHandlerDev>();
                //services.AddScoped<ISomeHelperService, SomeHelperDevService>();
            }
            else
            {
                //services.AddScoped<ISomeHandler, SomeHandler>();
                //services.AddScoped<ISomeHelperService, SomeHelperService>();
            }

            #endregion


            //services.AddSwaggerGen();
            #region Regsier Swagger Generator
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<JsonMergePatchDocumentOperationFilter>();
                c.SwaggerDoc("v1", new Info { Title = "POC API", Version = "v1" });
                c.EnableAnnotations();
                c.DescribeAllParametersInCamelCase();
            });
            #endregion

            #region Register the Swagger generator (other way)
            /*
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "POC API",
                    //Description = "A simple example Web API",
                    //TermsOfService = "None",
                    //Contact = new Contact  {  Name = "Some Name", Email = "Some.Name@abc123.com", Url = "https://twitter.com/someAbc" },
                    //License = new License  { Name = "Use under LICX", Url = "https://example.com/license"}
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(@"C:\Developer\Repos\POCs\POC_SeleniumEtc\POC_01\Src\POC.API\bin\Debug\netcoreapp2.1\POC.API.xml");
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
                c.OperationFilter<AddCommonHttpRequestHeadersInSwagger>();

            });

            */
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Register middleware 
            // app.UseMiddleware<ExceptionHandlingMiddleware>(); //for MVC ??
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();

            app.UseStaticFiles();

            //app.UseMiddleware(typeof(CommonHttpResponseHandlerMiddleware));
            app.UseMiddleware<CommonHttpResponseHandlerMiddleware>();

            app.UseSwagger();
            //app.UseSwaggerUI();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API POC");
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                //c.InjectStylesheet("/swagger-ui/custom.css");
            });

            app.UseMvc();
        }
    }
}
