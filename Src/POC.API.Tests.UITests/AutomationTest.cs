using System;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using POC.API.Tests.UITests.Helpers;
//using OpenQA.Selenium.PhantomJS;

namespace POC.API.Tests.UITests
{
    public abstract class AutomationTest : AutomationBase
    {
        protected LogonHelper Logon;
        //protected MenuHelper Menu;
        //protected RoundHelper Round;
        //protected StudentHelper Student;
        //protected FinancialAssessmentHelper FinancialAssessment;
        //protected CoeHelper Coe;
        //protected ProviderHelper Provider;
        //protected PaymentHelper Payment;
        //protected RefundHelper Refund;
        //protected RegistrationHelper Registration;


       public AutomationTest()
        {
            Setup();
            //if (IsSingleSession)
            GotoHomePage();
        }

        //[TestFixtureSetUp]
        //public void DoTestFixtureSetup()
        //{
        //    Setup();
        //    //if (IsSingleSession)
        //        GotoHomePage();
        //}

        [SetUp]
        public void SetupTest()
        {
            //if (!IsSingleSession)
                GotoHomePage();
        }

        private void Setup()
        {
            Driver = GetDriver();
            Logon = new LogonHelper(Driver);
            //Menu = new MenuHelper(Driver);
            //Round = new RoundHelper(Driver);
            //Student = new StudentHelper(Driver);
            //Coe = new CoeHelper(Driver);
            //Provider = new ProviderHelper(Driver);
            //Payment = new PaymentHelper(Driver);
            //Refund = new RefundHelper(Driver);
            //Registration = new RegistrationHelper(Driver);
            //FinancialAssessment = new FinancialAssessmentHelper(Driver, Coe, Logon);

            // wait for 5 seconds if looking for an element
            Driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 5);
            //Driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
        }

        //[TearDown]
        //public void TeardownTest()
        //{
        //    TakeScreenshotIfNecessary();
        //    //if (!IsSingleSession)
        //        Quit();
        //}

        //[TestFixtureTearDown]
        //public void TeardownSingleSessionTest()
        //{
        //    if (IsSingleSession)
        //        Quit();
        //}

        private void TakeScreenshotIfNecessary()
        {
            //if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
                TakeScreenshot();
            //else
            //    ClearScreenshot();
        }

        private static IWebDriver GetDriver()
        {
            string browser = "ie";// ConfigurationManager.AppSettings["browser"];
            switch (browser.Trim().ToLower())
            {
                case "chrome":
                case "google":
                    var chromeOptions = new ChromeOptions();
                    // Copy and unzip the folder I:\BS Branch\50015681\IITS\PRISMS\Developer\Chrome into C:\Developer\Chrome
                    chromeOptions.BinaryLocation = @"C:\developer\Chrome\chrome.exe";
                    return new ChromeDriver(chromeOptions);
                case "ie":
                case "internetexplorer":
                    var options = new InternetExplorerOptions();
                    // avoid the "Protected Mode must be set to the same value (enabled or disabled) for all zones" error
                    //options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    options.IgnoreZoomLevel = true;
                    //var options = new InternetExplorerDriver(@"Drivers\");

                    return new InternetExplorerDriver(@"Drivers\", options);
                    //return new InternetExplorerDriver(options);
                case "ff":
                case "firefox":
                case "mozilla":
                    return new FirefoxDriver();
                //case "phantom":
                //    return new PhantomJSDriver();
                default:
                    throw new ArgumentException(string.Format("Browser '{0}' is not recognised", browser));
            }
        }

    }
}
