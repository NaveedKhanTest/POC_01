using System;
using System.Collections.Generic;
using System.Text;

namespace POC.API.Tests.IntegrationTests
{
    public static class Configurations
    {
        private static string apiVersion = "v1";
        private static string valuesUrlBase = $"api/Values/";

        public static string ApiVersion { get => apiVersion; set => apiVersion = value; }

        public static string ValuesUrlBase { get => valuesUrlBase; set => valuesUrlBase = value; }
    }
}
