using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Application.Services
{
    public class RpaService : IRpaService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<RpaService> _logger;

        public RpaService(ICourseRepository courseRepository, ILogger<RpaService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync(string termoBusca)
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                using var driver = new ChromeDriver(options);

                driver.Navigate().GoToUrl("https://www.alura.com.br/");

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                var searchBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("header-barraBusca-form-campoBusca")));
                searchBox.SendKeys(termoBusca);
                searchBox.Submit();

                var advancedFilterButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".show-filter-options")));
                advancedFilterButton.Click();

                var courseFilterLabel = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".busca--filtro")));
                courseFilterLabel.Click();

                var searchButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".busca-form-botao.--desktop")));
                searchButton.Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".busca-resultado")));

                bool hasNextPage = true;
                while (hasNextPage)
                {
                    wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".busca-resultado")));

                    var cursosElements = driver.FindElements(By.CssSelector(".busca-resultado"));

                    foreach (var element in cursosElements)
                    {
                        await CollectCourseData(driver, element, wait);
                    }

                    try
                    {
                        var nextButton = driver.FindElement(By.CssSelector(".busca-paginacao-prevNext.busca-paginacao-linksProximos"));
                        if (nextButton.GetAttribute("class").Contains("busca-paginacao-prevNext--disabled"))
                        {
                            hasNextPage = false;
                        }
                        else
                        {
                            nextButton.Click();
                            wait.Until(ExpectedConditions.StalenessOf(cursosElements.First()));
                            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".busca-resultado")));
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        hasNextPage = false;
                    }
                }

                driver.Quit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar a automação.");
                throw;
            }
        }

        private async Task CollectCourseData(IWebDriver driver, IWebElement element, WebDriverWait wait)
        {
            string link = null;
            try
            {
                var linkElement = element.FindElement(By.CssSelector(".busca-resultado-link"));
                link = linkElement.GetAttribute("href");
            }
            catch (NoSuchElementException)
            {
                _logger.LogInformation("Link não encontrado para um dos cursos. Pulando este curso.");
                return;
            }

            if (!link.StartsWith("https://www.alura.com.br/"))
            {
                return;
            }

            string title;
            try
            {
                title = element.FindElement(By.CssSelector(".busca-resultado-nome")).Text;
            }
            catch (NoSuchElementException)
            {
                title = "Desconhecido";
            }

            string description;
            try
            {
                description = element.FindElement(By.CssSelector(".busca-resultado-descricao")).Text;
            }
            catch (NoSuchElementException)
            {
                description = "Desconhecido";
            }

            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl(link);

            wait.Until(d => d.Url.StartsWith(link));

            if (driver.FindElements(By.CssSelector(".erro-cta-button")).Any())
            {
                _logger.LogInformation($"Rpa: {link} página não encontrada");
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.First());
                return;
            }

            string hours;
            try
            {
                hours = driver.FindElement(By.CssSelector(".courseInfo-card-wrapper-infos")).Text;
            }
            catch (NoSuchElementException)
            {
                try
                {
                    hours = driver.FindElement(By.CssSelector(".formacao__info-destaque")).Text;
                }
                catch (NoSuchElementException)
                {
                    hours = "Desconhecido";
                }
            }

            string teacher;
            try
            {
                teacher = driver.FindElement(By.CssSelector(".instructor-title--name")).Text;
            }
            catch (NoSuchElementException)
            {
                try
                {
                    teacher = driver.FindElement(By.CssSelector(".formacao-instrutor-nome")).Text;
                }
                catch (NoSuchElementException)
                {
                    teacher = "Desconhecido";
                }
            }

            var curso = new Course()
            {
                Title = title,
                Description = description,
                Hours = hours,
                Teacher = teacher
            };

            await _courseRepository.AddAsync(curso);

            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }
    }
}
