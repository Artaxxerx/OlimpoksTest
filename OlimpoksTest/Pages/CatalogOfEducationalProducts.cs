using System.Diagnostics;
using System.Text;
using OlimpoksTest.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

class CatalogOfEducationalProducts : BasePage
{
    private By workersInput = By.XPath("//text()[contains(.,'Рабочие')]/following-sibling::input");
    private By laborProtection = By.XPath("//text()[contains(.,'Инструктаж')]/following-sibling::input");
    private By videoTutorialsCourses = By.XPath("//div[text()='15 курсов']");
    private By coursesCatalog = By.XPath("//div[contains(@class,'product-card_courses') and contains(@style,'display: block;')]");
    private By courseName = By.XPath(".//div[contains(@class, 'course-card_name')]");
    private By courseCode = By.XPath(".//div[contains(@class, 'course-card_code-number')]");
    private By courseModalWindow = By.XPath("//*[@id=\"products_container\"]/div[7]/div[2]/div[1]/div/div[3]");
    private By courseUpdatesTab = By.XPath("//button[contains(text(), 'Обновления')]");
    private By courseUpdateList = By.XPath("//*[@class='course-update-card']");
    private By popupClose = By.ClassName("telegram-popup__close");
    private By closeModalWindow = By.XPath("//*[@id=\"course-detail\"]/div/div/div[1]/button"); // Добавлен XPath для закрытия модального окна
    private readonly string outputDirectory;

    public CatalogOfEducationalProducts(IWebDriver driver) : base(driver)
    {
        string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        outputDirectory = Path.Combine(projectRoot, "Output");
        Directory.CreateDirectory(outputDirectory);
    }

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

    public CatalogOfEducationalProducts PopupClose()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(popupClose));
        driver.FindElement(popupClose).Click();
        return this;
    }

    public CatalogOfEducationalProducts OpenAListOfCourses()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        var element = wait.Until(d => d.FindElement(videoTutorialsCourses));
        element.Click();
        return this;
    }

    public void ProcessAllCourses()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        var courseElements = wait.Until(d => d.FindElements(coursesCatalog));

        foreach (var courseElement in courseElements)
        {
            var name = courseElement.FindElement(courseName).Text;
            var code = courseElement.FindElement(courseCode).Text;
           
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", courseElement);
           
            var detailsButton = wait.Until(d =>
            {
                var btn = courseElement.FindElement(courseModalWindow);
                return (btn.Displayed && btn.Enabled) ? btn : null;
            });
            detailsButton.Click();

          
            var updatesTab = wait.Until(d =>
            {
                var tab = d.FindElement(courseUpdatesTab);
                return (tab.Displayed && tab.Enabled) ? tab : null;
            });
            updatesTab.Click();

            
            int updatesCount = driver.FindElements(courseUpdateList).Count;

            
            File.WriteAllText(
                Path.Combine(outputDirectory, $"{SanitizeFileName(name)} ({code}) - {updatesCount}.txt"),
                $"Курс: {name}\nШифр: {code}\nОбновлений: {updatesCount}",
                Encoding.UTF8
            );

            // Закрытие модального окна
            var closeButton = wait.Until(d => d.FindElement(closeModalWindow));
            closeButton.Click();
              
        }

        Process.Start("explorer.exe", outputDirectory);
    }
      private string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return new string(fileName
            .Where(c => !invalidChars.Contains(c))
            .ToArray())
            .Replace(" ", "_");
    }
}
