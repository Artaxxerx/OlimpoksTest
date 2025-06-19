using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OlimpoksTest.Pages;

namespace OlimpoksTest.Tests
{
    public class MaxNumberOfCourses : BaseTest
    {
        [Fact]
        public void TestMaxNumberOfCourses()
        {
            var mainPageTest = new MainPage(driver);
            var catalogOfEducationProd = new CatalogOfEducationalProducts(driver);

            mainPageTest
                .ClickDecisionButton()
                .ClickLaborProtectionButton();

            catalogOfEducationProd
                .PopupClose()
                .FindProductWithMaximumNumberOfCourses()
                .CountTheMaximumNumberOfCourses();

            Assert.Equal(112, catalogOfEducationProd.CountTheMaximumNumberOfCourses());
        }
    }
}
