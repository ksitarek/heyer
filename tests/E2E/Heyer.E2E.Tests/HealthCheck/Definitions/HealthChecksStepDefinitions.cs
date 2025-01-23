using Shouldly;
using Heyer.BuildingBlocks.Json;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using OpenQA.Selenium;
using Reqnroll;

namespace Heyer.E2E.Tests.HealthCheck.Definitions;

[Binding]
public class HealthChecksStepDefinitions : IDisposable
{
    private readonly WebDriverProvider _p = WebDriverProvider.Create();
    private string _api = null!;

    public void Dispose() => _p.Dispose();

    [Given(@"the (.*) API is running")]
    public void GivenTheHeyerApiIsRunning(string api) => _api = api;

    [Then(@"the API should be healthy")]
    public void ThenTheApiShouldBeHealthy()
    {
        var json = _p.Driver.FindElement(By.TagName("pre")).Text;
        var health = json.Deserialize<HealthReport>()!;

        health.Status.ShouldBe(HealthCheckStatus.Healthy);
    }

    [When(@"I check the healthcheck endpoint")]
    public void WhenICheckTheHealthcheckEndpoint()
    {
        switch (_api)
        {
            case "Heyer":
                _p.Driver.Navigate().GoToUrl(RuntimeSettings.ApiAddress + "/health");
                break;

            case "Storage":
                _p.Driver.Navigate().GoToUrl(RuntimeSettings.StorageApiAddress + "/health");
                break;
        }
    }
}