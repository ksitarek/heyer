using FluentResults;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.PublishedLanguage.DTOs;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.List;

public record GetListHandler : IRequestHandler<GetList, Result<IEnumerable<PublishedJobOfferListItem>>>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;

    public GetListHandler(IPublishedJobOffersRepository publishedJobOffersRepository) =>
        _publishedJobOffersRepository = publishedJobOffersRepository;

    public async Task<Result<IEnumerable<PublishedJobOfferListItem>>> Handle(GetList request,
                                                                             CancellationToken cancellationToken)
    {
        var jobOffers = await _publishedJobOffersRepository.GetPageAsync(0, 10, cancellationToken);

        return Result.Ok(jobOffers.Select(x => x.MapToJobOfferListItem()));
    }
}