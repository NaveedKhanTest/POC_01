using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace POC.API.Tests.UITests
{
    public abstract class AutomationBase
    {
        protected const string FileForUpload = "test.doc";

        private const int numberOfAttempts = 10;
        private readonly int defaultTimeout;
        private readonly int pause;
        private const string screenshotsPath = "..\\..\\screenshots\\";

        protected AutomationBase()
        {
            defaultTimeout = 50;// int.Parse(ConfigurationManager.AppSettings["timeout"]);
            pause = 0; // int.Parse(ConfigurationManager.AppSettings["pause"]);
        }

        protected IWebDriver Driver { get; set; }

        private bool CanDriverHandleAlerts
        {
            get { return true; }
            //get { return !(Driver is PhantomJSDriver); }
        }

        private bool CanDriverHandleJavascriptRedirects
        {
            // phantom will not wait for a redirect to complete if the redirect was triggered by javascript
            get { return true; }
            //get { return !(Driver is PhantomJSDriver); }
        }

        protected object ExecuteScript(string script)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
            return TryToGet(() => executor.ExecuteScript(script));
        }

        protected void TakeScreenshot(string fileSuffix = "")
        {
            var camera = Driver as ITakesScreenshot;
            if (camera != null)
            {
                //string[] testPath = TestContext.CurrentContext.Test.FullName.Split('.');
                //fileSuffix = !string.IsNullOrWhiteSpace(fileSuffix) ? "_" + fileSuffix : "";
                //Thread.Sleep(500);
                //string fileName = string.Format("{2}{0}_{1}{3}.png", testPath[testPath.Length - 2], testPath[testPath.Length - 1], screenshotsPath, fileSuffix);

                string fileName = string.Format("{0}{1}_{2}.png", screenshotsPath, "Screenshot",  fileSuffix);
                
                if (File.Exists(fileName))
                    File.Delete(fileName);
                camera.GetScreenshot().SaveAsFile(fileName);
                //camera.GetScreenshot().SaveAsFile(fileName, ImageFormat.Png);
            }
        }

        //protected void ClearScreenshot()
        //{
        //    string[] testPath = TestContext.CurrentContext.Test.FullName.Split('.');
        //    Thread.Sleep(500);
        //    string fileName = string.Format(
        //        "{2}{0}_{1}.png",
        //        testPath[testPath.Length - 2],
        //        testPath[testPath.Length - 1], screenshotsPath);
        //    if (File.Exists(fileName))
        //        File.Delete(fileName);
        //}

        //protected bool IsSingleSession
        //{
        //    get { return ConfigurationManager.AppSettings["single-session"] == "true"; }
        //}

        private void Pause()
        {
            if (pause > 0)
                Thread.Sleep(pause);
        }

        /// <summary>
        /// Does the same as driver.FindElement, but will retry if it fails.
        /// This is useful because FindElement is prone to intermittent failures.
        /// </summary>
        protected IWebElement FindElement(By by)
        {
            Pause();
            try
            {
                return TryToGet(() => Driver.FindElement(by));
            }
            catch (NoSuchElementException)
            {
                throw new ArgumentException(string.Format("Could not find element {0}", by));
            }
        }

        protected IWebElement FindElement(IWebElement parentElement, By by)
        {
            Pause();
            try
            {
                return TryToGet(() => parentElement.FindElement(by));
            }
            catch (NoSuchElementException)
            {
                throw new ArgumentException(string.Format("Could not find element {0} inside parent element {1}", by, parentElement));
            }
        }

        protected IEnumerable<IWebElement> FindElements(By by)
        {
            Pause();
            return TryToGet(() => Driver.FindElements(by));
        }

        protected IEnumerable<IWebElement> FindElements(IWebElement parentElement, By by)
        {
            Pause();
            return TryToGet(() => parentElement.FindElements(by));
        }

        protected bool ElementVisible(By by)
        {
            if (!ElementExists(by))
                return false;
            return FindElement(by).Displayed;
        }

        protected bool ElementExists(By by)
        {
            return FindElements(by).Any();
        }

        protected void ClickElement(By by)
        {
            TryToPerform(() =>
            {
                IWebElement[] elements = FindElements(by).Where(e => e.Displayed).ToArray();
                if (elements.Length == 0)
                    throw new ArgumentException(string.Format("Could not find a visible element by {0}", by));
                for (int i = elements.Length - 1; i >= 0; i--)
                {
                    IWebElement element = elements[i];
                    try
                    {
                        ClickElement(element);
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        // with dialogs in particular, we can have a situation where an element is displayed
                        // but is not clickable
                    }
                }
                // failed to click all our elements, so let the exception bubble out
                ClickElement(elements[0]);
            });
        }

        protected void ClickElement(IWebElement element)
        {
            Pause();
            TryToPerform(element.Click);
        }

        protected void ChooseSelectItem(By by, string dropdownItemText)
        {
            ChooseSelectItem(FindElement(by), dropdownItemText);
        }

        protected void ChooseSelectItem(By by, int indexOfItemToChoose)
        {
            ChooseSelectItem(FindElement(by), indexOfItemToChoose);
        }

        protected void ChooseSelectItem(IWebElement dropdownElement, string dropdownItemText)
        {
            SelectElement selectElement = new SelectElement(dropdownElement);
            TryToPerform(() => selectElement.SelectByText(dropdownItemText));
        }

        protected void ChooseSelectItem(IWebElement dropdownElement, int indexOfItemToChoose)
        {
            SelectElement selectElement = new SelectElement(dropdownElement);
            TryToPerform(() => selectElement.SelectByIndex(indexOfItemToChoose));
        }

        protected void ChooseSelectItemByPartialText(By by, string partialText)
        {
            ChooseSelectItemByPartialText(FindElement(by), partialText);
        }

        protected void ChooseSelectItemByPartialText(IWebElement dropdownElement, string partialText)
        {
            SelectElement selectElement = new SelectElement(dropdownElement);
            IList<IWebElement> options = TryToGet(() => selectElement.Options);
            IWebElement match = options.FirstOrDefault(o => o.Text.ToLower().Contains(partialText.ToLower()));
            if (match == null)
                throw new ArgumentException(string.Format("Could not find an option containing the text '{0}'", partialText));
            string value = TryToGet(() => match.GetAttribute("value"));
            TryToPerform(() => selectElement.SelectByValue(value));
        }

        private IWebElement FindDialogButton(string buttonText)
        {
            var buttons = FindElements(By.XPath(string.Format("//div[@class='modal-footer']//button[contains(.,'{0}')]", buttonText)))
                .Where(e => e.Displayed)
                .ToArray();
            if (buttons.Length == 0)
            {
                buttons = FindElements(By.XPath(string.Format("//div[@class='modal-footer']//a[contains(.,'{0}')]", buttonText)))
                .Where(e => e.Displayed)
                .ToArray();
            }
            switch (buttons.Length)
            {
                case 0:
                    throw new ArgumentException(string.Format("Could not find a visible dialog button with button text '{0}'", buttonText));
                case 1:
                    return buttons[0];
                default:
                    throw new ArgumentException(string.Format("Found more than one visible dialog button with button text '{0}'", buttonText));
            }
        }

        protected void ClickDialogButton(string buttonText)
        {
            TryToPerform(() =>
            {
                IWebElement button = FindDialogButton(buttonText);
                ClickElement(button);
            });
        }

        protected void SetFileForUpload(string idOfFileInput)
        {
            Pause();
            WaitForAllAjaxCallsToComplete();
            // styling of these on the student pages causes WebDriver to think that the file input element is not clickable, so remove styling first
            ExecuteScript("$('span.file-wrapper').removeClass('file-wrapper');");
            IWebElement input = FindElement(By.Id(idOfFileInput));
            string file = GetPathForAFileToUpload();
            TryToPerform(() => input.SendKeys(file));
        }

        protected void SendKeys(By by, string text)
        {
            IWebElement element = FindElement(by);
            SendKeys(element, text);
        }

        protected void SendKeys(IWebElement element, string text)
        {
            Pause();
            TryToPerform(element.Clear);
            TryToPerform(() => element.SendKeys(text));
        }

        protected void SendKeys(params string[] keys)
        {
            Pause();
            TryToPerform(() => PerformSendKeys(keys));
        }

        private void PerformSendKeys(string[] keys)
        {
            var actions = new Actions(Driver);
            foreach (string key in keys)
            {
                actions.SendKeys(key);
            }
            actions.Perform();
        }

        protected IWebElement GetListing()
        {
            IWebElement[] tables = FindElements(By.ClassName("deewrGrid")).ToArray();
            switch (tables.Length)
            {
                case 0:
                    throw new ArgumentException("Could not find a table");
                case 1:
                    return tables[0];
                default:
                    throw new ArgumentException(string.Format("Found {0} tables on the page. Use GetListingByColumnHeading or GetListingByContainerId instead", tables.Length));
            }
        }

        protected IWebElement GetListingByColumnHeading(string withColumnHeadingText)
        {
            IEnumerable<IWebElement> tables = FindElements(By.ClassName("deewrGrid"));
            IWebElement table = tables.SingleOrDefault(t => withColumnHeadingText == null || FindElements(t, By.CssSelector("th")).Any(e => e.Text == withColumnHeadingText));
            if (table == null)
                throw new ArgumentException(string.Format("Could not find a table with a column with a header of '{0}'", withColumnHeadingText ?? "anything"));
            return table;
        }

        protected IWebElement GetListingByContainerId(string containerId)
        {
            return FindElement(By.XPath(string.Format("//div[@id='{0}']//table", containerId)));
        }

        protected int GetListingRowCount(string containerId)
        {
            if (FindElements(By.XPath(string.Format("//div[@id='{0}']//tbody/tr/td[contains(@class, 'no-records-found')]", containerId))).Any())
                return 0;
            return FindElements(By.XPath(string.Format("//div[@id='{0}']//tbody/tr[not(contains(@class,'notselectable'))]", containerId))).Count();
        }

        protected void SortByColumnInListing(string listingContainerId, int columnNumber)
        {
            // this *should* work, but doesn't; gives an "element is not clickable" error...
            //var listing = GetListingByContainerId(listingContainerId);
            //IWebElement columnHeader = FindElement(listing, By.XPath(string.Format("//thead/tr/th[{0}]", columnNumber - 1)));
            //ClickElement(columnHeader);
            // ...thus this hack
            ExecuteScript(string.Format("$('th:eq({1})', $('#{0}')).click();", listingContainerId, columnNumber - 1));
            WaitForAllAjaxCallsToComplete();
        }

        protected void ClickRowInListing(int rowNumber, string listingContainerId = null, bool waitForAjaxCallsToComplete = false, bool waitForRedirectToComplete = false)
        {
            string containerXPath =
                !string.IsNullOrEmpty(listingContainerId)
                    ? string.Format("//div[@id='{0}']", listingContainerId)
                    : "";
            ClickElement(By.XPath(string.Format("{0}//table[contains(@class,'deewrGrid')]/tbody/tr[{1}]/td[1]", containerXPath, rowNumber)));
            if (waitForRedirectToComplete)
                WaitForRedirectToComplete();
            if (waitForAjaxCallsToComplete)
                WaitForAllAjaxCallsToComplete();
        }

        protected void ClickRowInListing(string cellContents, string listingContainerId = null, bool waitForAjaxCallsToComplete = false, bool waitForRedirectToComplete = false)
        {
            string containerXPath =
                !string.IsNullOrEmpty(listingContainerId)
                    ? string.Format("//div[@id='{0}']", listingContainerId)
                    : "";
            ClickElement(By.XPath(string.Format("{0}//table[contains(@class,'deewrGrid')]/tbody//td[contains(., '{1}')]", containerXPath, cellContents)));
            if (waitForRedirectToComplete)
                WaitForRedirectToComplete();
            if (waitForAjaxCallsToComplete)
                WaitForAllAjaxCallsToComplete();
        }

        protected void SetCheckbox(By by, bool check)
        {
            SetCheckbox(FindElement(by), check);
        }

        protected void SetCheckbox(IWebElement checkbox, bool check)
        {
            bool isChecked = checkbox.GetAttribute("checked") != null;
            if (isChecked != check)
                ClickElement(checkbox);
        }

        protected void ShouldNotHaveErrored()
        {
            IsTextPresent("A problem has occurred").Should().BeFalse();
        }

        protected void ShouldBeTextPresent(string text)
        {
            WaitForTextPresent(text, true);
            IsTextPresent(text).Should().BeTrue("should have found the text \"{0}\" on the page", text);
        }

        protected void ShouldNotBeTextPresent(string text)
        {
            WaitForTextPresent(text, false);
            IsTextPresent(text).Should().BeFalse("should not have found the text \"{0}\" on the page", text);
        }

        private void WaitForTextPresent(string text, bool present)
        {
            // sometimes text may take a moment to appear, e.g. while waiting for a dialog animation to complete
            for (int i = 0; i < numberOfAttempts; i++)
            {
                if (IsTextPresent(text) == present)
                    break;
                Thread.Sleep(500);
            }
        }

        protected void ShouldHaveRowCount(string tableContainerId, int expectedRowCount)
        {
            GetListingRowCount(tableContainerId).Should().Be(expectedRowCount);
        }

        protected bool IsTextPresent(string text)
        {
            return FindElement(By.XPath("/html/body")).Text.Contains(text);
        }

        protected void ChooseMultiSelectItems(string id, params string[] valuesToBeSelected)
        {
            // we call the select2 plug-in directly because selecting using clicks and sendkeys doesn't work with phantom
            string js = string.Format("$('#{0}').select2('val', {1});", id, ConvertToJsArray(valuesToBeSelected));
            ExecuteScript(js);
        }

        protected void ChooseMultiSelectItemsInContext(string id, string parentContextId, params string[] valuesToBeSelected)
        {
            // we call the select2 plug-in directly because selecting using clicks and sendkeys doesn't work with phantom
            string js = string.Format("$('#{0}', $('#{2}')).select2('val', {1});", id, ConvertToJsArray(valuesToBeSelected), parentContextId);
            ExecuteScript(js);
        }

        protected void ClearMultiSelect(string id)
        {
            ChooseMultiSelectItems(id);
        }

        private IWebElement ClickMultiSelect(string id)
        {
            IWebElement multiSelect = FindElement(By.Id("s2id_" + id));
            ClickElement(multiSelect);
            return multiSelect;
        }

        private string ConvertToJsArray(string[] source)
        {
            string result = string.Empty;
            for (int i = 0; i < source.Length; i++)
            {
                string item = source[i];
                if (i > 0)
                    result += ", ";
                result += string.Format("'{0}'", item);
            }
            return string.Format("[{0}]", result);
        }

        protected void SendTabKey()
        {
            SendKeys(Keys.Tab);
        }

        protected bool IsAlertPresent()
        {
            if (CanDriverHandleAlerts)
            {
                try
                {
                    Driver.SwitchTo().Alert();
                    return true;
                }
                catch (NoAlertPresentException)
                {
                    return false;
                }
            }
            else
            {
                return IsDummyAlertPresent();
            }
        }

        protected void AcceptAlert(IAlert alert)
        {
            TryToPerform(alert.Accept);
        }

        protected void ShouldBeAlertContainingText(string expectedAlertText)
        {
            WaitForAlert();
            if (CanDriverHandleAlerts)
            {
                IAlert alert = Driver.SwitchTo().Alert();
                alert.Text.Should().Contain(expectedAlertText);
                alert.Dismiss();
            }
            else
            {
                GetDummyAlertText().Should().Contain(expectedAlertText);
                DismissDummyAlert();
            }
        }
        protected void WaitForAlert()
        {
            SetupDummyAlertHandler();
            WaitFor(IsAlertPresent, defaultTimeout, "alert to appear");
        }

        private void SetupDummyAlertHandler()
        {
            if (!CanDriverHandleAlerts)
            {
                ExecuteScript("window.alert = function(message) { lastAlert = message; }");
            }
        }

        private bool IsDummyAlertPresent()
        {
            return !string.IsNullOrWhiteSpace(GetDummyAlertText());
        }

        private string GetDummyAlertText()
        {
            return (string)ExecuteScript("return lastAlert;");
        }

        private void DismissDummyAlert()
        {
            ExecuteScript("lastAlert=null;");
        }

        protected void WaitForTextPresent(string text)
        {
            WaitFor(() => IsTextPresent(text), defaultTimeout, string.Format("text '{0}' to be present", text));
        }

        protected void WaitForTextNotPresent(string text)
        {
            WaitFor(() => !IsTextPresent(text), defaultTimeout, string.Format("text '{0}' to not be present", text));
        }

        protected void WaitForRedirectToComplete()
        {
            if (!CanDriverHandleAlerts)
            {
                string originalUrl = Driver.Url;
                WaitFor(() => Driver.Url != originalUrl, 5, string.Format("page to move off {0}", originalUrl));
            }
        }
        protected void WaitForAllAjaxCallsToComplete()
        {
            WaitForAllAjaxCallsToComplete(defaultTimeout);
        }

        protected void WaitForAllAjaxCallsToComplete(int secondsToWait)
        {
            WaitFor(() => GetNumberOfAjaxCalls() == 0, secondsToWait, "ajax calls to complete");
        }

        private static void WaitFor(Func<bool> condition, int secondsToWait, string description)
        {
            DateTime end = DateTime.Now.AddSeconds(secondsToWait);
            do
            {
                if (condition())
                    return;
                Thread.Sleep(500);
            } while (DateTime.Now < end);
            Assert.Fail(string.Format("Timed out after {0} seconds waiting for {1}", secondsToWait, description));
        }

        private long GetNumberOfAjaxCalls()
        {
            long numberOfAjaxCalls = (long)ExecuteScript("return jQuery.active;");
            return numberOfAjaxCalls;
        }

        private static T TryToGet<T>(Func<T> get)
        {
            for (int i = 0; i < numberOfAttempts; i++)
            {
                try
                {
                    return get();
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            return get();
        }

        private static void TryToPerform(Action action)
        {
            for (int i = 0; i < numberOfAttempts; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
            action();
        }

        protected void Quit()
        {
            try
            {
                Driver.Quit();
            }
            catch
            {
                // Ignore errors if unable to close the browser
            }
        }

        protected void Wait(int secondsToWait)
        {
            Thread.Sleep(secondsToWait * 1000);
        }

        protected void WaitMilliseconds(int millisecondsToWait)
        {
            Thread.Sleep(millisecondsToWait);
        }

        protected string GetPathForAFileToUpload()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string directory = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            return Path.Combine(directory, FileForUpload);
        }

        protected string GetUniqueEmailAddress()
        {
            return string.Format("{0:ddMMMyyyyhhmmssfff}@automationtesting.com", DateTime.Now);
        }

        protected IEnumerable<IWebElement> GetAllVisibleTabs()
        {
            return FindElements(By.XPath("//a[@data-toggle='tab']")).Where(e => e.Displayed);
        }

        protected string GetValueFromPanelRow(string labelCaption)
        {
            var caption = FindElement(By.XPath(string.Format("//div[@class='panel-row']/div[text()='{0}']/following-sibling::div[1]", labelCaption)));
            return caption.Text;
        }

        protected void ScrollToTop()
        {
            ExecuteScript("window.scrollTo(0,0)");
        }

        protected void GotoHomePage()
        {
            GotoUrl("");
        }

        protected void GotoUrl(string url)
        {
            Driver.Navigate().GoToUrl("https://katalon-demo-cura.herokuapp.com");
            //Driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["site"] + url);
            Wait(1);
        }

        protected string GetSampleText()
        {
            return string.Format("Generated by automation test at {0}", DateTime.Now);
        }
    }
}
