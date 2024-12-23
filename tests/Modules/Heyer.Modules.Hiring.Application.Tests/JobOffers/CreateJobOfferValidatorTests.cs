using FluentAssertions;
using FluentValidation.TestHelper;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.PublishedLanguage;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class CreateJobOfferValidatorTests
{
    private const string TooLongOfferSummary =
        "leutzowmnireuzitefewwwmxjfbhoaokztnskokbyaqewzjjlobovdyeethytiiqhhjhaytckwzbtjleleattpnjbsllxnvmjqfjlsuuzlbrovwnazlsepdyobortwstvdvrtyurponpsxmgqmhppqdtnegjgtfkwgkrbqjzvrxcxvishbszuplmxjaatzcgfeqwtwskcatdgmmvsfpuchqzjdklxskgkowuasmjgmuyjhqguqmpgmivodioezxrsnrslwfmcwareichjryhiooxcbuqxsivszdcrolumtvefzassubzvrlclyhqftrogljvqkledzygnpripqdgqwkhpflyygpdnwhdktkjynkrfiffmpqdkozqbkahionjkybfpzqzadblcmvwpsjbwdviljwvveocmupncdvdeqhqdxugorzghuqyfanypnffwvxzurrcfeljqubqwhwetkvmwc";

    private const string TooShortJobDescription = "wyqxu";
    private const string TooShortOfferSummary = "rm";

    private const string ValidJobDescription =
        "saacpecnienlzckquxujarumjjothtjzewflnjfrjcsemhdqxjybttlurbgkongmaorbedvmoocpibvbwxfljntgcvplywqgipiryoxapjvphpwtrqmuqavehdkjvadf";

    private const string ValidOfferSummary = "umcxmhkhhdvqmmyklfjx";
    private CreateJobOfferValidator _validator;

    public static IEnumerable<object[]> JobDescriptionErrorTestCases()
    {
        yield return
        [
            TooShortJobDescription,
            $"The length of 'Job Description' must be at least 100 characters. You entered {TooShortJobDescription.Length} characters."
        ];
        yield return ["", "'Job Description' must not be empty."];
        yield return [null!, "'Job Description' must not be empty."];
    }

    public static IEnumerable<object[]> OfferSummaryErrorTestCases()
    {
        yield return
        [
            TooShortOfferSummary,
            $"The length of 'Offer Summary' must be at least 10 characters. You entered {TooShortOfferSummary.Length} characters."
        ];
        yield return
        [
            TooLongOfferSummary,
            $"The length of 'Offer Summary' must be 100 characters or fewer. You entered {TooLongOfferSummary.Length} characters."
        ];
        yield return ["", "'Offer Summary' must not be empty."];
        yield return [null!, "'Offer Summary' must not be empty."];
    }

    public static IEnumerable<object[]> RemoteWorkErrorTestCases()
    {
        yield return [RemoteWork.Unknown, "Remote work must be specified."];
        yield return [null!, "Remote work must be specified."];
    }

    [SetUp]
    public void SetUp() => _validator = new CreateJobOfferValidator();

    [Theory]
    [TestCaseSource(nameof(JobDescriptionErrorTestCases))]
    public void Validate_ShouldFailWhenJobDescriptionIsInvalid(string input, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateJobOffer(ValidOfferSummary, input, RemoteWork.Hybrid);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.JobDescription)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [TestCaseSource(nameof(OfferSummaryErrorTestCases))]
    public void Validate_ShouldFailWhenOfferSummaryIsInvalid(string input, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateJobOffer(input, ValidJobDescription, RemoteWork.Hybrid);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.ShouldHaveValidationErrorFor(x => x.OfferSummary)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [TestCaseSource(nameof(RemoteWorkErrorTestCases))]
    public void Validate_ShouldFailWhenRemoteWorkIsInvalid(RemoteWork input, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateJobOffer(ValidOfferSummary, ValidJobDescription, input);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.RemoteWork)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Test]
    public void Validate_ShouldSucceed()
    {
        // Arrange
        var request = new CreateJobOffer(ValidOfferSummary, ValidJobDescription, RemoteWork.Hybrid);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}