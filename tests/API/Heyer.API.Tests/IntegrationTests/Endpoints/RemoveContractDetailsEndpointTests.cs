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
public class RemoveContractDetailsEndpointTests : IntegrationTestsBase
{
    private JobOfferId _jobOfferId;

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
        var response = await action.Should().NotThrowAsync();

        await using var ctx = GetContext(ApplicationFactoryConfiguration.Client1Id);

        var jobOffer = await ctx.JobOffers.AsNoTracking().FirstAsync(x => x.Id == _jobOfferId);

        jobOffer.ContractsDetails!
            .Any(x => x.EmploymentType == request.EmploymentType).Should().BeFalse();
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
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
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
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

[Category("Integration")]
public class UpdateContractDetailsEndpointTests : IntegrationTestsBase
{
    private JobOfferId _jobOfferId;

    public static IEnumerable<object[]> BadRequestTestCases()
    {
        yield return new object[]
        {
            new UpdateContractDetailsRequest(Guid.Empty, EmploymentType.B2B, new SalaryRange(true, 1, 2), 8, 8)
        };
        yield return new object[]
        {
            new UpdateContractDetailsRequest(Guid.NewGuid(), default, new SalaryRange(true, 1, 2), 8, 8)
        };
        yield return new object[]
        {
            new UpdateContractDetailsRequest(Guid.NewGuid(), EmploymentType.B2B, new SalaryRange(), 8, 8)
        };
        yield return new object[]
        {
            new UpdateContractDetailsRequest(Guid.NewGuid(), EmploymentType.B2B, new SalaryRange(true, 1, 0), 8, 8)
        };
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


    [Test]
    public async Task UpdateContractDetails_WillReturn200()
    {
        // Arrange
        var client =
            _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id,
                                                  HiringPermissions.UpdateJobOffer);

        var request = new UpdateContractDetailsRequest(
            _jobOfferId.Guid,
            EmploymentType.ContractOfEmployment,
            new SalaryRange(true, 2, 3),
            1,
            1);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        await action.Should().NotThrowAsync();

        await using var ctx = HiringDbContextProvider.Get(ApplicationFactoryConfiguration.Client1Id);

        var jobOffer = await ctx.JobOffers
            .Include(x => x.ContractsDetails)
            .FirstAsync(x => x.Id == _jobOfferId);

        var contractDetails = jobOffer.ContractsDetails!.First(x => x.EmploymentType == request.EmploymentType);

        contractDetails.SalaryRange.From.Should().Be(request.SalaryRange.From);
        contractDetails.SalaryRange.To.Should().Be(request.SalaryRange.To);
        contractDetails.SalaryRange.IsPublished.Should().Be(request.SalaryRange.IsPublished);
        contractDetails.TimeNumerator.Should().Be(request.TimeNumerator);
        contractDetails.TimeDenominator.Should().Be(request.TimeDenominator);
    }

    [Theory]
    [TestCaseSource(nameof(BadRequestTestCases))]
    public async Task UpdateContractDetails_WillReturn400(UpdateContractDetailsRequest request)
    {
        // Arrange
        var client =
            _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id,
                                                  HiringPermissions.UpdateJobOffer);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task UpdateContractDetails_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();
        var request = new UpdateContractDetailsRequest(
            JobOfferId.CreateNew().Guid,
            EmploymentType.ContractOfEmployment,
            new SalaryRange(false, 1, 2),
            8,
            8);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task UpdateContractDetails_WillReturn403()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id);

        var request = new UpdateContractDetailsRequest(
            JobOfferId.CreateNew().Guid,
            EmploymentType.ContractOfEmployment,
            new SalaryRange(false, 1, 2),
            8,
            8);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task UpdateContractDetails_WillReturn404()
    {
        // Arrange
        var client =
            _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client1Id,
                                                  HiringPermissions.UpdateJobOffer);

        var request = new UpdateContractDetailsRequest(
            JobOfferId.CreateNew().Guid,
            EmploymentType.ContractOfEmployment,
            new SalaryRange(false, 1, 2),
            8,
            8);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task UpdateContractDetails_WillReturn404_ForOtherTenant()
    {
        // Arrange
        var client =
            _appFactory.CreateAuthorizedApiClient(ApplicationFactoryConfiguration.Client2Id,
                                                  HiringPermissions.UpdateJobOffer);

        var request = new UpdateContractDetailsRequest(
            _jobOfferId.Guid,
            EmploymentType.ContractOfEmployment,
            new SalaryRange(false, 1, 2),
            8,
            8);

        // Act
        var action = async () => await client.UpdateContractDetails(request);

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private HiringDbContext GetContext(Guid clientId) => HiringDbContextProvider.Get(clientId);
}