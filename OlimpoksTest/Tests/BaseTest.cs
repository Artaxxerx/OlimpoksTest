using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace OlimpoksTest.Tests
{
    public class BaseTest : IDisposable
    {
        protected IWebDriver driver {  get; }

        public BaseTest() {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://olimpoks.ru/");
        }
        public void Dispose()
        {
            // Закрываем браузер и освобождаем ресурсы
            driver?.Quit();
            driver?.Dispose();
        }

    }
}
