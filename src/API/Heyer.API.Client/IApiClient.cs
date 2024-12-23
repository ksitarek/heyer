using Heyer.Modules.Hiring.PublishedLanguage;
using RestEase;

namespace Heyer.API.Client;

public interface IApiClient
{
    [Get("/job-board/{jobOfferId}")]
    Task<PublishedJobOfferDetails> GetJobOfferById([Path("jobOfferId")] Guid jobOfferId);

    [Post("/job-offers/create")]
    Task<Guid> CreateJobOffer([Body] CreateJobOfferRequest createJobOfferRequestRequest);

    [Get("/health")]
    [AllowAnyStatusCode]
    Task<Response<HealthReport>> HealthCheck();
}