
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1&tabs=aspnetcore2x

Use the built-in logging API and providers in your code for dotnet core.
you can plug in a third-party logging framework like NLog

sample code: https://github.com/aspnet/Docs/tree/master/aspnetcore/fundamentals/logging/index/samples/2.x/TodoApiSample




You need to use "Microsoft.Extensions.Logging.AzureAppServices" package and then register the logging provider for azure using code below.

 loggerFactory.AddAzureWebAppDiagnostics( 
    new AzureAppServicesDiagnosticsSettings 
    {
          OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss zzz} [{Level}] {RequestId}-{SourceContext}: {Message}{NewLine}{Exception}" 
    } 
  );

  To enable diagnostics in the Azure portal, go to the page for your web app and click Settings > Diagnostics logs.

  Download logs 
  ftp://waws-prod-dm1-027.ftp.azurewebsites.windows.net
