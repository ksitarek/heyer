using FluentResults;
using Shouldly;

namespace Heyer.BuildingBlocks.Tests.Extensions;

[ShouldlyMethods]
public static class AssertionsExtensions
{
    public static void ShouldBeFailure<T>(this T actual,
                                          string? expectedError = null,
                                          string? expectedReason = null,
                                          string? customMessage = null)
        where T : ResultBase
    {
        actual.IsFailed.ShouldBeTrue(customMessage);

        if (expectedError is not null)
        {
            actual.Errors.ShouldContain(x => x.Message == expectedError, customMessage);

            if (expectedReason is not null)
            {
                actual.Errors.SelectMany(x => x.Reasons)
                    .ShouldContain(x => x.Message == expectedReason, customMessage);
            }
        }
    }

    public static void ShouldBeSuccess<T>(
        this T actual,
        string? customMessage = null)
        where T : ResultBase =>
        actual.IsSuccess.ShouldBeTrue(customMessage);

    public static void ShouldBeSuccess<T>(
        this Result<T> actual,
        T expectedValue,
        string? customMessage = null)
    {
        actual.IsSuccess.ShouldBeTrue(customMessage);
        actual.Value.ShouldBe(expectedValue, customMessage);
    }

    public static void ShouldBeWithin(this DateTime actual, TimeSpan expected, string? customMessage = null) =>
        actual.ShouldBeGreaterThanOrEqualTo(DateTime.UtcNow.Subtract(expected), customMessage);

    public static void ShouldContainSingle<T>(this IEnumerable<T> actual,
                                              Func<T, bool> predicate,
                                              string? customMessage = null)
        where T : class =>
        actual.SingleOrDefault(predicate).ShouldNotBeNull(customMessage);

    public static void ShouldContainSingle<T>(this IEnumerable<T> actual,
                                              T expected,
                                              string? customMessage = null)
        where T : class =>
        actual.SingleOrDefault(expected).ShouldNotBeNull(customMessage);

    public static void ShouldContainSingle<T>(this IEnumerable<T> actual,
                                              string? customMessage = null)
        where T : class =>
        actual.SingleOrDefault().ShouldNotBeNull(customMessage);

    public static void ShouldHaveError<TError>(this ResultBase actual, string? customMessage = null)
        where TError : Error =>
        actual.Errors.ShouldContainSingle(x => x is TError, customMessage);

    public static void ShouldHaveException<TException>(this ResultBase actual,
                                                       Func<TException, bool> predicate,
                                                       string? customMessage = null)
        where TException : Exception =>
        actual.Errors
            .OfType<ExceptionalError>()
            .Any(e => e.Exception is TException ex && predicate(ex))
            .ShouldBeTrue(customMessage);

    public static void ShouldNotBeEmpty(this Guid actual, string? customMessage = null) =>
        actual.ShouldNotBe(Guid.Empty, customMessage);
}