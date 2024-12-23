using Heyer.Modules.Hiring.PublishedLanguage;
using RestEase;

namespace Heyer.API.Client;

public interface IApiClient
{
    [Post("/job-offers/create")]
    Task<Guid> CreateJobOffer([Body] CreateJobOfferRequest createJobOfferRequestRequest);

    [Get("/job-offers/{jobOfferId}")]
    Task<JobOfferDetails> GetJobOfferById([Path("jobOfferId")] Guid jobOfferId);

    [Get("/job-board/{jobOfferId}")]
    Task<PublishedJobOfferDetails> GetPublishedJobOfferById([Path("jobOfferId")] Guid jobOfferId);

    [Get("/health")]
    [AllowAnyStatusCode]
    Task<Response<HealthReport>> HealthCheck();
}