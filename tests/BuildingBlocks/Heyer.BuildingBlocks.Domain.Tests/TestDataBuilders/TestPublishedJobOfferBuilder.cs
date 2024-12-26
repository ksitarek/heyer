using Bogus;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;

internal class TestPublishedJobOfferBuilder
{
    internal static readonly Faker _f = new();
    private readonly Guid _companyId;

    private TestPublishedJobOfferBuilder(Guid companyId) => _companyId = companyId;

    internal static Randomizer R => _f.Random;
    public static TestPublishedJobOfferBuilder Create(Guid companyId) => new(companyId);

    public PublishedJobOffer BuildTestData()
    {
        var publishedJobOfferId = PublishedJobOfferId.CreateNew();

        var companyDetails = new CompanyDetails(_companyId, _f.Random.Words(2));

        var offerSummary = _f.Random.Words(5);

        var jobDescription = _f.Random.Words(100);

        var remoteWork = _f.PickRandom(RemoteWork.Unknown);

        var contractsDetails = new ContractDetails(
            _f.PickRandom<EmploymentType>(),
            new SalaryRange(R.Bool(),
                            _f.Random.Number(10000, 20000),
                            _f.Random.Number(20000, 30000)),
            _f.Random.Number(1, 8),
            _f.Random.Number(1, 8));

        var location = new OfficeLocation(_f.Address.City(), _f.Address.Country());

        var publishedUntil = _f.Date.FutureOffset();

        var requirements = new Requirements(
            R.Enum<ExperienceLevel>(),
            new Faker<SkillKvp>()
                .RuleFor(x => x.Label, f => f.Random.Words(f.Random.Number(1, 2)))
                .RuleFor(x => x.Level, f => f.Random.Enum<SkillLevel>())
                .Generate(4)
                .Select(x => new Skill(x.Label, x.Level))
                .ToList());

        return PublishedJobOffer.CreateNew(
            publishedJobOfferId,
            companyDetails,
            offerSummary,
            jobDescription,
            remoteWork,
            [contractsDetails],
            location,
            publishedUntil,
            requirements);
    }

    private class SkillKvp
    {
        public string Label { get; set; } = null!;
        public SkillLevel Level { get; set; }
    }
}