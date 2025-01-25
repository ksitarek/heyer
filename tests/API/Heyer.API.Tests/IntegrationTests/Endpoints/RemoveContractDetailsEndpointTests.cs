using System.Net;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.EntityFrameworkCore;
using RestEase;
using Shouldly;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class RemoveContractDetailsEndpointTests : IntegrationTestsBase
{
    private JobOfferId _jobOfferId = null!;

    [Test]
    public async Task RemoveContractDetailsEndpoint_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var request = new RemoveContractDetailsRequest(_jobOfferId.Guid, EmploymentType.ContractOfEmployment);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        await action.ShouldNotThrowAsync();

        await using var ctx = GetContext(ApplicationFactoryConfiguration.Client1Id);

        var jobOffer = await ctx.JobOffers.AsNoTracking().FirstAsync(x => x.Id == _jobOfferId);

        jobOffer.ContractsDetails!
            .Any(x => x.EmploymentType == request.EmploymentType).ShouldBeFalse();
    }

    [Theory]
    [TestCase("00000000-0000-0000-0000-000000000000", EmploymentType.B2B)]
    [TestCase("ABA7EA0D-F08C-43CF-8083-486C9B610707", default(EmploymentType))]
    public async Task RemoveContractDetailsEndpoint_WillReturn400(string guid, EmploymentType employmentType)
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var request = new RemoveContractDetailsRequest(Guid.Parse(guid), employmentType);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task RemoveContractDetailsEndpoint_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var request = new RemoveContractDetailsRequest(Guid.NewGuid(), EmploymentType.ContractOfEmployment);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task RemoveContractDetailsEndpoint_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id);
        var request = new RemoveContractDetailsRequest(Guid.NewGuid(), EmploymentType.ContractOfEmployment);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task RemoveContractDetailsEndpoint_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var request = new RemoveContractDetailsRequest(Guid.NewGuid(), EmploymentType.ContractOfEmployment);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task RemoveContractDetailsEndpoint_WillReturn404_ForOtherTenant()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client2Id,
            HiringPermissions.UpdateJobOffer);

        var request = new RemoveContractDetailsRequest(_jobOfferId.Guid, EmploymentType.ContractOfEmployment);

        // Act
        var action = async () => await client.RemoveContractDetails(request);

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [SetUp]
    public async Task SetUp()
    {
        await using var ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Client1Id);

        var jobOffer = TestJobOfferBuilder.Create()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        jobOffer.AddContractDetails(
            new ContractDetails(EmploymentType.ContractOfEmployment, new SalaryRange(false, 1, 2), 8, 8));

        _jobOfferId = jobOffer.Id;

        await ctx.Set<JobOffer>().AddAsync(jobOffer);

        await ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await using var ctx = GetContext(ApplicationFactoryConfiguration.Client1Id);

        ctx.Set<JobOffer>().RemoveRange(await ctx.JobOffers.ToListAsync());

        await ctx.SaveChangesAsync();
    }

    private HiringDbContext GetContext(Guid clientId) => HiringDbContextProvider.Get(clientId);
}