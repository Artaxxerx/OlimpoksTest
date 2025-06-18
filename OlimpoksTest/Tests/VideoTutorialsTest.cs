using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OlimpoksTest.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace OlimpoksTest.Tests
{
    public class VideoTutorialsTest : BaseTest
    {
        [Fact]
        public void DownloadVideoTutorialsCourses()
        {
            var mainPageTest = new MainPage(driver);
            var catalogOfEducationProd = new CatalogOfEducationalProducts(driver);

            mainPageTest
                .ClickDecisionButton()
                .ClickLaborProtectionButton();

            catalogOfEducationProd
                .ClickWorkersInput()
                .ClickLaborProtectionInput()
                .OpenAListOfCourses()
                .ProcessAllCourses();
        }
    }
}
