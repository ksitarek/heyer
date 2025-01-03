using Bogus;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;

internal class TestJobOfferBuilder
{
    internal static readonly Faker _f = new();
    private readonly List<ContractDetails> _contractsDetails = new();
    private readonly string _jobDescription;
    private readonly string _offerSummary;
    private readonly RemoteWork _remoteWork;
    private ExperienceLevel? _experienceLevel;
    private OfficeLocation? _location;
    private Dictionary<string, SkillLevel>? _skills;

    private TestJobOfferBuilder()
    {
        _offerSummary = R.Words(5);
        _jobDescription = R.Words(100);
        _remoteWork = R.Enum(RemoteWork.Unknown);
    }

    internal static Randomizer R => _f.Random;

    public static TestJobOfferBuilder Create() => new();

    public JobOffer Build()
    {
        var jobOffer = JobOffer.CreateNew(
            JobOfferId.CreateNew(),
            _offerSummary,
            _jobDescription,
            _remoteWork);

        if (_experienceLevel != null && _skills != null)
        {
            jobOffer.SetRequirements(_experienceLevel.Value, _skills);
        }

        if (_location != null)
        {
            jobOffer.SetOfficeLocation(_location);
        }

        if (_contractsDetails.Any())
        {
            foreach (var contractDetails in _contractsDetails)
            {
                jobOffer.AddContractDetails(contractDetails);
            }
        }

        return jobOffer;
    }

    public TestJobOfferBuilder WithRandomContractDetails()
    {
        _contractsDetails.Add(new ContractDetails(
                                  _f.PickRandom<EmploymentType>(),
                                  new SalaryRange(R.Bool(),
                                                  _f.Random.Number(10000, 20000),
                                                  _f.Random.Number(20000, 30000)),
                                  _f.Random.Number(1, 8),
                                  _f.Random.Number(1, 8)));

        return this;
    }

    public TestJobOfferBuilder WithRandomOfficeLocation()
    {
        _location = new OfficeLocation(_f.Address.City(), _f.Address.Country());
        return this;
    }

    public TestJobOfferBuilder WithRandomRequirements()
    {
        _experienceLevel = R.Enum<ExperienceLevel>();
        _skills = new Faker<SkillKvp>()
            .RuleFor(x => x.Label, f => f.Random.Words(f.Random.Number(1, 2)))
            .RuleFor(x => x.Level, f => f.Random.Enum<SkillLevel>())
            .Generate(4)
            .ToDictionary(x => x.Label, x => x.Level);

        return this;
    }

    private class SkillKvp
    {
        public string Label { get; set; } = null!;
        public SkillLevel Level { get; set; }
    }
}