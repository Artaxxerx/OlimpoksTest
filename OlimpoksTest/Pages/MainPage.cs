using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OlimpoksTest.Pages
{
     class MainPage : BasePage
    {

        private By decisionButton = By.XPath("//*[text()='Решения']");
        private By laborProtection = By.XPath("//div[text()='Охрана труда']");

        public MainPage(IWebDriver driver) : base(driver) { }

     

        public MainPage ClickDecisionButton()
        {
            driver.FindElement(decisionButton).Click();
            return this;
        }
        public MainPage ClickLaborProtectionButton()
        {
            driver.FindElement(laborProtection).Click();
            return this;
        }
    }
}
