Implement Azure Redis Cache 

https://www.youtube.com/watch?v=PNqWEXBQSHc

Install Nuget pack "StackExchange.Redis"

appsettings.json => Right click => Properties => Copy if newer 


---------------

First step is to use asp net core session state example

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1

2nd step is in to do it using Azure Redis Cache


ASP.NET session state in the cache


https://docs.microsoft.com/en-us/azure/redis-cache/cache-aspnet-session-state-provider

Install-Package Microsoft.Web.RedisSessionStateProvider
