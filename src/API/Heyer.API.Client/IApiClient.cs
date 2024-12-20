using Heyer.API.Client.PublishedLanguage;
using RestEase;

namespace Heyer.API.Client;

public interface IApiClient
{
    [Post("/job-offers/create")]
    Task<Guid> CreateJobOffer([Body] CreateJobOfferRequest createJobOfferRequestRequest);

    [Get("/health")]
    [AllowAnyStatusCode]
    Task<Response<HealthReport>> HealthCheck();
}