using FluentResults;
using Heyer.Storage.API.Client.PublishedLanguage;
using MediatR;

namespace Heyer.Storage.API.Store;

public record StoreRequest(IFormFile File) : IRequest<Result<StoreResult>>;