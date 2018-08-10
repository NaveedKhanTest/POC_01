using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;

namespace POC.API.Tests.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private static TestServer server;

        public static HttpClient HttpClient { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext tc)
        {
            var webHostBuilder = new WebHostBuilder()
            .UseEnvironment("Test") // (Development, Staging, Production)
            .UseStartup<Startup>(); // Startup class of web api project

            //Error : Could not load file or assembly 'Microsoft.AspNetCore.Mvc,
            //To Fix the error: https://stackoverflow.com/questions/50401152/integration-and-unit-tests-no-longer-work-on-asp-net-core-2-1-failing-to-find-as

            server = new TestServer(webHostBuilder);
            HttpClient = server.CreateClient();
        }

        [TestInitialize]
        public void Initialize()
        {
        }

        [ClassInitialize]
        public void ClassInitialize()
        {
            // run code before you run the first test in the class
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [ClassCleanup]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed, template project only")]
        public static void ClassCleanup()
        {
            // Cleanup to run code after all tests in a class have run
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            HttpClient.Dispose();
            server.Dispose();
            HttpClient = null;
            server = null;
        }

        public ApiResponseMessage<T> GetById<T>(string url)
            where T : class
        {
            var apiResponseMessage = new ApiResponseMessage<T>();

            // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    //var content = response.Content.ReadAsAsync<LinkModelWrapper<T>>().Result;
                    //apiResponseMessage.SingleItemOutputWrapper = content;
                    //apiResponseMessage.Item = content.Value;
                    apiResponseMessage.IsSuccessful = response.IsSuccessStatusCode;
                }

                apiResponseMessage.StatusCode = response.StatusCode;
            }

            return apiResponseMessage;
        }

        public ApiResponseMessage<T> GetAll<T>(string url)
            where T : class
        {
            var apiResponseMessage = new ApiResponseMessage<T>();

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    apiResponseMessage.ListItems = response.Content.ReadAsAsync<List<T>>().Result;
                    //apiResponseMessage.ListOutputWrapper = content;
                    //apiResponseMessage.ListItems = content.Values;
                    apiResponseMessage.IsSuccessful = response.IsSuccessStatusCode;
                }

                apiResponseMessage.StatusCode = response.StatusCode;
            }

            return apiResponseMessage;
        }

        public ApiResponseMessage<T> AddNewRecord<T>(string url, T newItem)
        {
            var apiResponseMessage = new ApiResponseMessage<T>();

            using (var response = HttpClient.PostAsJsonAsync(url, newItem).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    apiResponseMessage.IsSuccessful = response.IsSuccessStatusCode;
                    apiResponseMessage.Location = response.Headers.Location;
                }

                apiResponseMessage.StatusCode = response.StatusCode;
            }

            return apiResponseMessage;
        }

        public ApiResponseMessage<T> PartiallyUpdate<T>(string url, T updatedItem)
        {
            var apiResponseMessage = new ApiResponseMessage<T>();
            var contentJson = JsonConvert.SerializeObject(updatedItem);
            var content = new StringContent(contentJson, Encoding.UTF8, "application/json");

            using (var response = HttpClient.PatchAsync(url, content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    apiResponseMessage.IsSuccessful = response.IsSuccessStatusCode;
                    apiResponseMessage.Location = response.Headers.Location;
                }

                apiResponseMessage.StatusCode = response.StatusCode;
            }

            return apiResponseMessage;
        }
    }
}
