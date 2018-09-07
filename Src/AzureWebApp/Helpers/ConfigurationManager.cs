using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureWebApp
{
    public class ConfigurationManager
    {
        public static string GetAppSettings(string key)
        {
            var config = InitConfiguration();
            var keyValue = config[key];
            return keyValue;
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

    }



}
