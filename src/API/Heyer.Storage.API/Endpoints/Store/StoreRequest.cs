using FluentResults;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Store;

public record StoreRequest(IFormFile File) : IRequest<Result<StoreResult>>;