using Heyer.API.Client.PublishedLanguage;
using RestEase;

namespace Heyer.API.Client;

public interface IApiClient
{
    [Post("/job-offers/create")]
    Task CreateJobOffer([Body]CreateJobOfferRequest createJobOfferRequestRequest);
}