using System.Net;
using FluentAssertions;
using Heyer.API.Tests.Utils;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Application;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.EntityFrameworkCore;
using RestEase;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class AddContractDetailsEndpointTests : IntegrationTestsBase
{
    private HiringDbContext _ctx;
    private JobOffer _jobOffer;

    public static IEnumerable<object[]> BadRequestTestCases()
    {
        yield return new object[] { new ContractDetails() };
        yield return new object[] { new ContractDetails(EmploymentType.B2B, new SalaryRange(), 8, 8) };
        yield return new object[] { new ContractDetails(EmploymentType.B2B, new SalaryRange(false, 0, 0), 8, 8) };
        yield return new object[] { new ContractDetails(EmploymentType.B2B, new SalaryRange(false, 1, 0), 8, 8) };

        // duplicate employment type
        yield return new object[]
        {
            new ContractDetails(EmploymentType.ContractOfEmployment, new SalaryRange(false, 1, 2), 8, 8)
        };
    }

    [Test]
    public async Task AddContractDetailsEndpoint_WillReturn200()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var contractDetails = new ContractDetails(
            EmploymentType.B2B,
            new SalaryRange(false, 1, 2),
            8,
            8);

        var request = new AddContractDetailsRequest(_jobOffer.Id.Guid, contractDetails);

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        await action.Should().NotThrowAsync();

        var jobOffer = await _ctx.JobOffers.AsNoTracking().FirstAsync(x => x.Id == _jobOffer.Id);
        jobOffer.ContractsDetails!
            .Any(x => x.EmploymentType == contractDetails.EmploymentType).Should().BeTrue();
    }

    [Theory]
    [TestCaseSource(nameof(BadRequestTestCases))]
    public async Task AddContractDetailsEndpoint_WillReturn400(ContractDetails contractDetails)
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var request = new AddContractDetailsRequest(_jobOffer.Id.Guid, contractDetails);

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task AddContractDetailsEndpoint_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client1Id,
            HiringPermissions.UpdateJobOffer);

        var contractDetails = new ContractDetails(
            EmploymentType.B2B,
            new SalaryRange(false, 1, 2),
            8,
            8);

        var request = new AddContractDetailsRequest(Guid.NewGuid(), contractDetails);

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task AddContractDetailsEndpoint_WillReturn404_ForOtherTenant()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(
            ApplicationFactoryConfiguration.Client2Id,
            HiringPermissions.UpdateJobOffer);

        var contractDetails = new ContractDetails(
            EmploymentType.B2B,
            new SalaryRange(false, 1, 2),
            8,
            8);

        var request = new AddContractDetailsRequest(_jobOffer.Id.Guid, contractDetails);

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task AddContractDetailsEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var request = new AddContractDetailsRequest(Guid.NewGuid(), new ContractDetails());

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task AddContractDetailsEndpoint_WithoutPermission_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id);
        var request = new AddContractDetailsRequest(Guid.NewGuid(), new ContractDetails());

        // Act
        var action = async () => await client.AddContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [SetUp]
    public async Task SetUp()
    {
        _ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Client1Id);

        _jobOffer = TestJobOfferBuilder.Create()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        _jobOffer.AddContractDetails(
            new ContractDetails(EmploymentType.ContractOfEmployment, new SalaryRange(false, 1, 2), 8, 8));

        await _ctx.Set<JobOffer>().AddAsync(_jobOffer);

        await _ctx.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        _ctx.Set<JobOffer>().Remove(_jobOffer);

        await _ctx.SaveChangesAsync();

        await _ctx.DisposeAsync();
    }
}