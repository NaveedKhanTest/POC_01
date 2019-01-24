using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Morcatko.AspNetCore.JsonMergePatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POC.API.Tests.IntegrationTests
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private static TestServer server;
        public TestContext TestContext { get; set; }

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

            //HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //HttpClient.DefaultRequestHeaders.Add("some-id", "25");
            //HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "authorization_Header Key 111 xxx");
            ////HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");

            ////Note:  headers per request
            ////If you don't want to set the header on the HttpClient instance by adding it to the DefaultRequestHeaders, 
            ////you could set headers per request.
            ////using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://your.site.com"))
            ////{
            ////    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", your_token);
            ////    httpClient.SendAsync(requestMessage);
            ////}

            SetCommonRequestHeaders();
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

        public async Task<HttpResponseMessage> Patch(string path, string json)
        {
            var content = new StringContent(json, System.Text.Encoding.UTF8, JsonMergePatchDocument.ContentType);
            var httpResponse = await HttpClient.PatchAsync(path, content).ConfigureAwait(false);
            return await httpResponse.CheckServerError(this.TestContext);
        }


        public async Task<HttpResponseMessage> Post(string path, object newItem)
        {
            //var content = new StringContent(json, System.Text.Encoding.UTF8, JsonMergePatchDocument.ContentType);
            var httpResponse = await HttpClient.PostAsJsonAsync(path, newItem).ConfigureAwait(false);
            return await httpResponse.CheckServerError(this.TestContext);
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

        //Name it Post
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

        private static void SetCommonRequestHeaders()
        {
            HttpClient.DefaultRequestHeaders.Add("request-timestamp", DateTime.UtcNow.ToString("s") + "Z");
            HttpClient.DefaultRequestHeaders.Add("api-key", "xxxx111222Key");
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            // HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "authorization key aaa");
        }
    }
}
