using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace OlimpoksTest.Pages
{
    class CatalogOfEducationalProducts : BasePage
    {
        private By workersInput = By.XPath("//text()[contains(.,'Рабочие')]/following-sibling::input");
        private By laborProtection = By.XPath("//text()[contains(.,'Инструктаж')]/following-sibling::input");
        private By videoTutorialsCourses = By.XPath("//*[@id=products_container']/div[7]/div[1]/div[1]/div[5]/a/following-sibling::button");
        private By coursesCatalog = By.XPath("//div[contains(@class,'product-card_courses') and contains(@style,'display: block;')]");
        private By courseName = By.XPath(".//div[contains(@class, 'course-card_name')]");
        private By courseCode = By.XPath(".//div[contains(@class, 'course-card_code-number')]");
        private By courseModalWindow = By.XPath("//div[contains(@class, 'modal') and @id='course-detail']");
        private By courseUpdatesTab = By.XPath("//button[contains(text(), 'Обновления')]");
        private By courseUpdateList = By.XPath("//*[@class='course-update-card']");
        private By modalClose = By.CssSelector(".telegram-popup__close");

        public CatalogOfEducationalProducts(IWebDriver driver) : base(driver) { }


        public CatalogOfEducationalProducts ClickWorkersInput()
        {
            driver.FindElement(workersInput).Click();
            return this;
        }
        public CatalogOfEducationalProducts ClickLaborProtectionInput()
        {
            driver.FindElement(laborProtection).Click();
            return this;
        }

        public CatalogOfEducationalProducts OpenAListOfCourses()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(d => d.FindElement(videoTutorialsCourses));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
            return this;
        }
        public void ProcessAllCourses()
        {
            
            var courseElements = driver.FindElements(coursesCatalog);
           

            foreach (var courseElement in courseElements)
            {

                var name = courseElement.FindElement(courseName).Text;
                var code = courseElement.FindElement(courseCode).Text;

                var detailsButton = courseElement.FindElement(courseModalWindow);
                detailsButton.Click();

                var updatesTab = driver.FindElement(courseUpdatesTab);
                updatesTab.Click();

                int updatesCount = 0;
                try
                {
                    updatesCount = driver.FindElements(courseUpdateList).Count;
                }
                catch (NoSuchElementException)
                {

                }

                var fileName = $"{name} ({code}) - {updatesCount}.txt";
                File.WriteAllText(fileName, $"Курс: {name}\nШифр: {code}\nОбновлений: {updatesCount}");
               
            }
        }
    }
}