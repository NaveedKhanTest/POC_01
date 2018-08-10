using System.Collections.Generic;
using OpenQA.Selenium;
using System.Linq;

namespace POC.API.Tests.UITests.Helpers
{
    public class LogonHelper : AutomationBase
    {
        public LogonHelper(IWebDriver driver)
        {
            Driver = driver;
        }


        public void OpenPage(string logonId = "superuser", string password = "Password1", bool waitForLogin = true, bool rememberLogonId = false)
        {
            //GotoUrl("/profile.php#login");
            //GotoUrl("/Home/Signout");

            

            ClickElement(By.Id("btn-make-appointment"));

            SendKeys(By.Id("txt-username"), "John Doe");
            SendKeys(By.Id("txt-password"), "ThisIsNotAPassword");
            ClickElement(By.Id("btn-login"));

            ClickElement(By.Id("menu-toggle"));

            TakeScreenshot("MyXyzFile");

            ClickElement(By.XPath("id('sidebar-wrapper')/ul[@class='sidebar-nav']/li[5]/a[1]"));
            //ExecuteScript("$('#menu-close').click();");
            //GotoUrl("/authenticate.php?logout");

            Quit();

            //authenticate.php?logout
            //menu-toggle


            //btn-make-appointment

            //LogOut();
            //GotoLoginPage();
            //SendKeys(By.Id("ProviderEmailAddress"), logonId);
            //SendKeys(By.Id("ProviderPassword"), password);
            //SetCheckbox(FindElement(By.Id("RememberProviderEmailAddress")), rememberLogonId);
            //ClickElement(By.Id("uxProviderLoginButton"));
            //if (waitForLogin)
            //{
            //    WaitForRedirectToComplete();
            //    WaitForAllAjaxCallsToComplete();
            //}
        }

        public void AsInternalUser(string logonId = "superuser", string password = "Password1", bool waitForLogin = true, bool rememberLogonId = false)
        {
            LogOut();
            GotoLoginPage();
            SendKeys(By.Id("ProviderEmailAddress"), logonId);
            SendKeys(By.Id("ProviderPassword"), password);
            SetCheckbox(FindElement(By.Id("RememberProviderEmailAddress")), rememberLogonId);
            ClickElement(By.Id("uxProviderLoginButton"));
            if (waitForLogin)
            {
                WaitForRedirectToComplete();
                WaitForAllAjaxCallsToComplete();
            }
        }

        //public void AsProvider(string organisationName = "macquarie")
        //{
        //    AsInternalUser("provider");
        //    new ProviderHelper(Driver).ChooseOrganisation(organisationName);
        //}

        public void AsStudent(string logonId, string password = "password", bool waitForLogin = true, bool rememberLogonId = false)
        {
            LogOut();
            GotoLoginPage();
            SendKeys(By.Id("StudentEmailAddress"), logonId);
            SendKeys(By.Id("StudentPassword"), password);
            SetCheckbox(FindElement(By.Id("RememberStudentEmailAddress")), rememberLogonId);
            ClickElement(By.Id("uxStudSignIn"));
            if (waitForLogin)
                WaitForAllAjaxCallsToComplete();
        }

        public void GotoLoginPage()
        {
            GotoUrl("/Home/Login");
        }

        public void LogOut()
        {
            GotoUrl("/Home/Signout");
        }

    }
}
