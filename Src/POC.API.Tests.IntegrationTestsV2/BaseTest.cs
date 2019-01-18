namespace POC.API.Tests.IntegrationTestsV2
{
    // Install-Package Microsoft.Extensions.Configuration.EnvironmentVariables -Version 2.1.1
    //No don't install it. Note add nuget for Microsoft.Extensions.Configuration

    //To fix AddJsonFile error add Microsoft.Extensions.Configuration.Json nuget or add ref of the api project

    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Base class for integration test that handles the httpServer and client.
    /// </summary>
    [TestClass]
    public abstract class BaseTest : BaseApiTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class.
        /// </summary>
        public BaseTest()
        {
        }

        /// <summary>
        /// Executes once before any test in the asembly is executed.
        /// </summary>
        /// <param name="testContext">The testContext.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Test") // (Development, Staging, Prod)
                .UseConfiguration(config)
                .ConfigureServices((services) =>
                {
                    //services.AddSingleton<ISomeHandler, SomeHandlerDev>();
                    //Note: in Startup we can do services.TryAddSingleton<ISomeHandler, SomeHandler>();// 
                    //using Microsoft.Extensions.DependencyInjection;
                })
                .UseStartup<Startup>(); // Startup class of web api project

            // to fix error we should have same nuget packages
            // I updated to Microsoft.AspNetCore.App for whole solution
            TestServer = new TestServer(webHostBuilder);
        }

        /// <summary>
        /// Executes once after all tests in the assembly have executed.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            TestServer?.Dispose();
            TestServer = null;
        }

        /// <summary>
        /// Logic to execute after all tests in the class have exectuted.
        /// </summary>
        [ClassCleanup]
        public void ClassCleanup()
        {
            // Cleanup to run code after all tests in a class have run
        }

        /// <summary>
        /// Logic to execute before the firs test in the class executes.
        /// </summary>
        [ClassInitialize]
        public virtual void ClassInitialize()
        {
            // run code before you run the first test in the class
        }

        /// <summary>
        /// Locig to execute after the test executes.
        /// </summary>
        [TestCleanup]
        public virtual void TestCleanup()
        {
        }

        /// <summary>
        /// Logic to execute before the test.
        /// </summary>
        [TestInitialize]
        public virtual void TestInitialize()
        {
        }
    }
}
