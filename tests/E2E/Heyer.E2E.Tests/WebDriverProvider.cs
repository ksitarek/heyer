using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Heyer.E2E.Tests;

public class WebDriverProvider : IDisposable
{
    private WebDriverProvider(WebDriver driver) => Driver = driver;

    public WebDriver Driver { get; }

    public static WebDriverProvider Create()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--disable-extensions");

        var driver = new ChromeDriver(options);

        return new WebDriverProvider(driver);
    }

    public void Dispose() => Driver.Dispose();
}