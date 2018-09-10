
NLog in asp.net core 2.0 web application

https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2

1) NuGet: "NLog.Web.AspNetCore" (Version 4.5+ )
   NuGet: "NLog"

2) Create a nlog.config (lowercase all) file in the root of your project.

3. Enable copy to bin folder nlog.config

4. Update program.cs (for .net core 2.1 https://github.com/NLog/NLog.Web/issues/300)

5. Configure appsettings.json 

6. Inject the ILogger in your controller: private readonly ILogger<HomeController> _logger;
