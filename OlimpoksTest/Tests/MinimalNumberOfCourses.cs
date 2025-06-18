using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OlimpoksTest.Pages;

namespace OlimpoksTest.Tests
{
    internal class MinimalNumberOfCourses : BaseTest
    {
        [Fact]
        public void TestMinimalNumberOfCourses()
        {
            var mainPageTest = new MainPage(driver);
            var catalogOfEducationProd = new CatalogOfEducationalProducts(driver);

            mainPageTest
                .ClickDecisionButton()
                .ClickLaborProtectionButton();
            catalogOfEducationProd.
                PopupClose()
                .
        }
}
