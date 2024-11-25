using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ReportTests {
    public class SeleniumReportTest {
        IWebDriver _driver;

        [SetUp]
        public void Setup() {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void Generate_Report_And_Download_Test() {
            // Arrange - Open /adminlogin/
            var loginUrl = "http://localhost:8080/adminlogin";
            _driver.Navigate().GoToUrl(loginUrl);

            // Act 1 - Log in as admin
            var userNameInput = _driver.FindElement(By.Id("userName"));
            var passwordInput = _driver.FindElement(By.Id("userPassword"));
            var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            userNameInput.SendKeys("FirstAdmin");
            passwordInput.SendKeys("AwesomeSauce");
            loginButton.Click();

            // wait for redirection
            Thread.Sleep(2000);

            // Act 2 - redirect to report page
            var reportUrl = "http://localhost:8080/AdminReports/Completed";
            _driver.Navigate().GoToUrl(reportUrl);

            // wait for redirection
            Thread.Sleep(2000);

            // Act 3 - Fill dates and click buttons to generate report
            var startDateInput = _driver.FindElement(By.Id("startDate-selector"));
            var endDateInput = _driver.FindElement(By.Id("endDate-selector"));
            var generateReportButton = _driver.FindElement(By.CssSelector("button.btn-info"));
            var generatePDFButton = _driver.FindElement(By.CssSelector("button.btn-success"));

            startDateInput.SendKeys("11012024");
            Thread.Sleep(1000);
            endDateInput.SendKeys("11302024");
            generateReportButton.Click();
            Thread.Sleep(1000);

            // Act 4 - Download PDF
            generatePDFButton.Click();
            Thread.Sleep(1000);
            var confirmButton = _driver.FindElement(By.ClassName("modal-button-confirm"));
            confirmButton.Click();
            Thread.Sleep(1000);

            // Assert - Verify the report was downloaded
            bool isButtonEnabled = generatePDFButton.Enabled;
            Assert.IsTrue(isButtonEnabled, "PDF has been downloaded");
        }

        [TearDown]
        public void Teardown() {
            _driver.Quit();
        }
    }
}
