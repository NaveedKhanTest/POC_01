using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace POC.API.Tests.UITests
{
    [TestClass]
    public class UnitTest1 : AutomationTest
    {
        //https://stackoverflow.com/questions/38605747/selenium-in-c-sharp-when-run-the-test-case-with-ie-browser 
        [TestMethod]
        public void TestIE()
        {
            // webDriver = new   InternetExplorerDriver(ConfigurationSettings.AppSettings["IDEServerPath"].ToString(), options);//Path of ur IE WebDriver,Here I stored it in a AppConfig File


            #region IE Driver path error fix

            //The IEDriverServer.exe(as well as ChromeDriver.exe) can be downloaded from:

            //http://selenium-release.storage.googleapis.com/index.html.

            //To get these to work with your Selenium tests, include the .exe in your test project, and set its properties to 'Copy Always'.


            #endregion


            //Tips: https://www.codeproject.com/articles/1078541/advanced-webdriver-tips-and-tricks-part?PageFlow=Fluid

            IWebDriver driver = new InternetExplorerDriver(@"Drivers\");

            driver.Navigate().GoToUrl("http://www.google.com");

            IWebElement element = driver.FindElement(By.Name("q"));

            element.SendKeys("Hello Selenium WebDriver!");

            element.Submit();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Logon.OpenPage();
            var no = 1;
            Assert.IsTrue(no == 1);
        }

        //[Test]
        //public void ShouldBeAbleToLogon()
        //{
        //    Logon.OpenPage();
        //    ShouldBeTextPresent("task board");
        //}


        //private void WaitUntilLoaded()
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        //    wait.Until((x) =>
        //    {
        //        return ((IJavaScriptExecutor)this.driver)
        //        .ExecuteScript("return document.readyState").Equals("complete");
        //    });
        //}




    }
}
