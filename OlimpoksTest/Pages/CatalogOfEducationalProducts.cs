using System.Diagnostics;
using System.Text;
using OlimpoksTest.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

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

        // Только здесь внесено изменение - перебираем конкретные существующие номера
        int[] validCourseNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 17 };

        foreach (int i in validCourseNumbers)  // Заменил for на foreach
        {
            Console.WriteLine($"Обработка курса #{i}");

            string baseXPath = $"//*[@id='products_container']/div[7]/div[2]/div[{i}]";
            var nameElement = wait.Until(d => d.FindElement(By.XPath(baseXPath + "/div/div[2]/div")));
            var codeElement = wait.Until(d => d.FindElement(By.XPath(baseXPath + "/div/div[1]/div[2]")));
            var modalButton = wait.Until(d => d.FindElement(By.XPath(baseXPath + "/div/div[3]/a[1]")));

            string name = nameElement.Text.Trim();
            string code = codeElement.Text.Trim();

            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, arguments[0].offsetTop)", modalButton);
            modalButton.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(closeModalWindow));

            var updatesTab = wait.Until(ExpectedConditions.ElementIsVisible(courseUpdatesTab));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", updatesTab);

            var updates = driver.FindElements(courseUpdateList);
            int updatesCount = updates.Count;

            string fileName = $"{SanitizeFileName(name)} ({code}) - {updatesCount}.txt";
            File.WriteAllText(Path.Combine(outputDirectory, fileName),
                $"Курс: {name}\nШифр: {code}\nОбновлений: {updatesCount}",
                Encoding.UTF8
            );

            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.Escape).Perform();
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
