using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Domain.Tests;

[Category("Unit")]
public class JobOfferTests
{
    [Test]
    public void AddCandidate_ShouldAddCandidate()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        var candidateId = CandidateId.CreateNew();

        // Act
        var result = jobOffer.AddCandidate(candidateId);

        // Assert
        result.Should().BeSuccess();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(CandidateApplied)
                           && ((CandidateApplied)domainEvent).JobOfferId == jobOffer.Id
                           && ((CandidateApplied)domainEvent).CandidateId == candidateId);
    }

    [Test]
    public void AddCandidate_ShouldNotAddSameCandidateTwice()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        var candidateId = CandidateId.CreateNew();
        jobOffer.AddCandidate(candidateId);

        // Act
        var result = jobOffer.AddCandidate(candidateId);

        // Assert
        result.Should().BeFailure($"Candidate with id: {candidateId} has already applied for this job offer.");
    }

    [Test]
    public void AddContractDetails_ShouldAddContractDetails()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        // Act
        var result = jobOffer.AddContractDetails(
            new ContractDetails(
                EmploymentType.ContractOfEmployment,
                new SalaryRange(true, 10000, 20000),
                8,
                8));

        // Assert
        result.Should().BeSuccess();

        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(ContractDetailsAdded)
                           && ((ContractDetailsAdded)domainEvent).JobOfferId == jobOffer.Id
                           && ((ContractDetailsAdded)domainEvent).EmploymentType ==
                           EmploymentType.ContractOfEmployment);
    }

    [Test]
    public void AddContractDetails_ShouldNotAddContractDetailsForEmploymentTypeTwice()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.AddContractDetails(
            new ContractDetails(
                EmploymentType.ContractOfEmployment,
                new SalaryRange(true, 10000, 20000),
                8,
                8));

        // Act
        var result = jobOffer.AddContractDetails(
            new ContractDetails(
                EmploymentType.ContractOfEmployment,
                new SalaryRange(true, 10000, 20000),
                8,
                8));

        // Assert
        result.Should().BeFailure();
    }

    [Test]
    public void JobOfferShouldCreate()
    {
        // Arrange

        // Act
        var jobOffer = CreateTestJobOffer();

        // Assert
        jobOffer.Should().NotBeNull();
        jobOffer.Id.Should().NotBeNull();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferCreated)
                           && ((JobOfferCreated)domainEvent).JobOfferId == jobOffer.Id);
    }

    [Test]
    public void Publish_ShouldNotPublish_WhenNoContractsDetails()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());

        // Act
        var result = jobOffer.Publish();

        // Assert
        result.Should().BeFailure("Job offer must have at least one contract details when publishing.");
    }

    [Test]
    public void Publish_ShouldNotPublish_WhenNoOfficeLocation()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());

        // Act
        var result = jobOffer.Publish();

        // Assert
        result.Should().BeFailure("Job offer must have location when publishing.");
    }

    [Test]
    public void Publish_ShouldNotPublish_WhenNoRequirements()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));

        // Act
        var result = jobOffer.Publish();

        // Assert
        result.Should().BeFailure("Job offer must have requirements when publishing.");
    }

    [Test]
    public void Publish_ShouldNotPublish_WhenOfferIsAlreadyPublic()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));
        jobOffer.Publish();

        // Act
        var result = jobOffer.Publish();

        // Assert
        result.Should().BeFailure("Job offer must not be public.");
    }

    [Test]
    public void Publish_ShouldNotPublish_WhenUntilDateIsInPast()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));

        // Act
        var result = jobOffer.Publish(DateTimeOffset.Now.AddDays(-1));

        // Assert
        result.Should().BeFailure("Published until date must not be in the past.");
    }

    [Test]
    public void Publish_ShouldPublish()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));

        // Act
        var result = jobOffer.Publish(DateTimeOffset.UtcNow.AddDays(1));

        // Assert
        result.Should().BeSuccess();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferPublished)
                           && ((JobOfferPublished)domainEvent).JobOfferId == jobOffer.Id);
    }

    [Test]
    public void RemoveContractDetails_FailsWhenNoContractDetails()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        // Act
        var result = jobOffer.RemoveContractDetails(EmploymentType.B2B);

        // Assert
        result.Should().BeFailure($"Job offer must have contract details for employment type: {EmploymentType.B2B}.");
    }

    [Test]
    public void RemoveContractDetails_ShouldFailWhenNoEmploymentTypeInOffer()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));

        // Act
        var result = jobOffer.RemoveContractDetails(EmploymentType.ContractOfEmployment);

        // Assert
        result.Should()
            .BeFailure(
                $"Job offer must have contract details for employment type: {EmploymentType.ContractOfEmployment}.");
    }

    [Test]
    public void RemoveContractDetails_ShouldRemoveContractDetailsByEmploymentType()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));

        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.ContractOfEmployment,
                                                        new SalaryRange(false, 12000, 22000),
                                                        8,
                                                        8));

        // Act
        var result = jobOffer.RemoveContractDetails(EmploymentType.B2B);

        // Assert
        result.Should().BeSuccess();

        jobOffer.ContractsDetails.Should()
            .NotContain(contractDetails => contractDetails.EmploymentType == EmploymentType.B2B);

        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(ContractDetailsRemoved)
                           && ((ContractDetailsRemoved)domainEvent).JobOfferId == jobOffer.Id
                           && ((ContractDetailsRemoved)domainEvent).EmploymentType == EmploymentType.B2B);
    }

    [Test]
    public void SetOfficeLocation_ShouldSetOfficeLocation()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        var location = new OfficeLocation("City", "CountryCode");

        // Act
        var result = jobOffer.SetOfficeLocation(location);

        // Assert
        result.Should().BeSuccess();
    }

    [Test]
    public void SetRequirements_ShouldSetRequirements()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        var experienceLevel = ExperienceLevel.Junior;

        var skills = new Dictionary<string, SkillLevel> { { "Skill", SkillLevel.NiceToHave } };

        // Act
        var result = jobOffer.SetRequirements(experienceLevel, skills);

        // Assert
        result.Should().BeSuccess();
    }

    [Test]
    public void TakeDown_ShouldNotTakeDown_WhenNotPublished()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        // Act
        var result = jobOffer.TakeDown();

        // Assert
        result.Should().BeFailure("Job offer must be published to take it down.");
    }

    [Test]
    public void TakeDown_ShouldNotTakeDownTwice()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));
        jobOffer.Publish(DateTimeOffset.UtcNow.AddDays(1));
        jobOffer.TakeDown();

        // Act
        var result = jobOffer.TakeDown();

        // Assert
        result.Should().BeFailure();
    }

    [Test]
    public void TakeDown_ShouldTakeDown()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();
        jobOffer.SetOfficeLocation(new OfficeLocation("City", "CountryCode"));
        jobOffer.SetRequirements(ExperienceLevel.Junior, new Dictionary<string, SkillLevel>());
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.B2B,
                                                        new SalaryRange(false, 10000, 20000),
                                                        8,
                                                        8));
        jobOffer.Publish(DateTimeOffset.UtcNow.AddDays(1));

        // Act
        var result = jobOffer.TakeDown();

        // Assert
        result.Should().BeSuccess();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferTakenDown)
                           && ((JobOfferTakenDown)domainEvent).JobOfferId == jobOffer.Id);
    }

    [Test]
    public void UpdateDescription_ShouldUpdateDescription()
    {
        // Arrange
        var jobOffer = CreateTestJobOffer();

        var offerSummary = "OfferSummary";
        var jobDescription = "JobDescription";

        // Act
        var result = jobOffer.UpdateDescription(offerSummary, jobDescription);

        // Assert
        result.Should().BeSuccess();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferDescriptionUpdated)
                           && ((JobOfferDescriptionUpdated)domainEvent).JobOfferId == jobOffer.Id);
    }

    private JobOffer CreateTestJobOffer()
    {
        var offerSummary = "OfferSummary";
        var jobDescription = "JobDescription";
        var remoteWork = RemoteWork.Yes;

        return JobOffer.CreateNew(
            JobOfferId.CreateNew(),
            offerSummary,
            jobDescription,
            remoteWork);
    }
}