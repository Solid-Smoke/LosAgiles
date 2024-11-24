using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    internal class ProductSearchSeleniumTests
    {
        private IWebDriver driver;
        private const string url = "http://localhost:8080";

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Searchbar_prevents_illegal_character_inputs()
        {
            var specialCharacters = new List<char> {'@', '!', '%', '^', '*', '(', ')', '=', '{', '}', '[', ']',
                    '|', '\\', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '~', '-', '+', '_', '$', '/', '&', '#' };

            var alllowedSpecialCharacters = new List<char> { '-', '+', '_', '$', '/', '&', '#' };
            var illegalCharacters = specialCharacters.Except<char>(alllowedSpecialCharacters).ToList();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var searchInput = wait.Until(display => display.FindElement(By.Id("product-search-bar")));

            foreach (var illegalChar in illegalCharacters)
            {
                searchInput.Clear();
                searchInput.SendKeys("Sample text" + illegalChar);

                var feedback = wait.Until(display => display.FindElement(By.Id("input-live-feedback")));
                Assert.IsTrue(feedback.Displayed, $"Searchbar allowed this" +
                    $"illegal character: {illegalChar}");
            }
        }
    }
}
