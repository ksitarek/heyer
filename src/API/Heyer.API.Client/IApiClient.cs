using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using RestEase;

namespace Heyer.API.Client;

public interface IApiClient : IDisposable
{
    [Post("/job-offers/add-contract-details")]
    Task AddContractDetails([Body] AddContractDetailsRequest addContractDetailsRequest);

    [Post("/job-offers/create")]
    Task<Guid> CreateJobOffer([Body] CreateJobOfferRequest createJobOfferRequestRequest);

    [Get("/job-offers/{jobOfferId}")]
    Task<JobOfferDetails> GetJobOfferById([Path("jobOfferId")] Guid jobOfferId);

    [Get("/job-board/{jobOfferId}")]
    Task<PublishedJobOfferDetails> GetPublishedJobOfferById([Path("jobOfferId")] Guid jobOfferId);

    [Get("/health")]
    [AllowAnyStatusCode]
    Task<Response<HealthReport>> HealthCheck();

    [Post("/job-offers/publish")]
    Task PublishJobOffer([Body] PublishJobOfferRequest publishJobOfferRequest);
}