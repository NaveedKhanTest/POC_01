using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC.API.Tests.UITests
{
    [TestFixture]
    public class LogonTests : AutomationTest
    {
        [Test]
        public void ShouldBeAbleToLogon()
        {
            Logon.AsInternalUser();
            ShouldBeTextPresent("task board");
        }

        [Test]
        public void ShouldBeAbleToLogonAsAProvider()
        {
            Logon.AsInternalUser("provider");
            ShouldBeTextPresent("your bank account details");
        }

        //[Test]
        //public void ShouldBeAbleToChooseAnOrganisationAfterLoggingOnAsAProvider()
        //{
        //    Logon.AsProvider();
        //}

        [Test]
        public void ShouldNotBeAbleToLogonWithADodgyUserId()
        {
            Logon.AsInternalUser("notarealuser@deewr.gov.au", waitForLogin: false);
            ShouldBeAlertContainingText("Your email address and/or password are incorrect");
            ShouldNotBeTextPresent("Current tasks");
        }

        [Test]
        public void ShouldBeAbleToLogout()
        {
            Logon.AsInternalUser();
            Logon.LogOut();
        }

        [Test]
        public void ShouldNotRememberLogonIdIfNotAsked()
        {
            Logon.AsInternalUser();
            Logon.LogOut();
            Logon.GotoLoginPage();
            var value = FindElement(By.Id("ProviderEmailAddress")).GetAttribute("value").Should();
            //FindElement(By.Id("ProviderEmailAddress")).GetAttribute("value").Should().BeBlank();
        }

        [Test]
        public void ShouldRememberLogonIdIfAsked()
        {
            Logon.AsInternalUser(rememberLogonId: true);
            Logon.LogOut();
            Logon.GotoLoginPage();
            FindElement(By.Id("ProviderEmailAddress")).GetAttribute("value").Should().Be("superuser");
        }

        //[Test]
        //public void ShouldBeAbleToRecoverStudentPasswordUsingQuestionAndAnswer()
        //{
        //    Logon.AsInternalUser();
        //    Menu.OpenRounds();
        //    RoundDetails round = Round.CreateAndActivate("Kangan", true);
        //    string studentEmailAddress = Student.GetStudentEmailAddresses(round.Name)[0];
        //    string studentDateOfBirth = Student.GetStudentDatesOfBirth(round.Name)[0];
        //    Logon.LogOut();
        //    Logon.AsStudent(studentEmailAddress);
        //    Student.ChangePassword();
        //    Logon.LogOut();
        //    Logon.GotoLoginPage();
        //    ClickElement(By.LinkText("Forgotten your username/password?"));
        //    ShouldBeTextPresent("Enter your details");
        //    SendKeys(By.Id("uxResetPasswordEmail"), (studentEmailAddress));
        //    SendKeys(By.Id("uxStudentDateofBirth"), (studentDateOfBirth));
        //    ClickElement(By.Id("uxPasswordResetStep1"));
        //    WaitForAllAjaxCallsToComplete();
        //    SendKeys(By.Id("uxResetPasswordAnswer"), "Mom");
        //    ClickElement(By.Id("uxPasswordResetStep2"));
        //    WaitForAlert();
        //    IAlert alert = Driver.SwitchTo().Alert();
        //    alert.Text.Should().Contain("Your new password has been sent to your email address");
        //    AcceptAlert(alert);
        //}

    }
}
