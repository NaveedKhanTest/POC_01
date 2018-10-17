using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace POC.API
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
            //services.AddSwaggerGen();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // https://dotnetcore.gaprogman.com/2017/12/07/giving-dwcheckapi-that-swagger/ 
            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore

            // Register the Swagger generator, defining 1 or more Swagger documents
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
            //app.UseMiddleware<CustomExceptionHandlingMiddleware>();

            app.UseStaticFiles();

            //app.UseMiddleware(typeof(CommonHttpResponseHandlerMiddleware));
            app.UseMiddleware<CommonHttpResponseHandlerMiddleware>();

            app.UseMvc();
            app.UseSwagger();
            //app.UseSwaggerUI();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                //c.InjectStylesheet("/swagger-ui/custom.css");
            });

            //app.UseSwaggerUI();
        }
    }
}
