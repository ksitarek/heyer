using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Heyer.Storage.API.Client.PublishedLanguage;

public record StoreRequest(IFormFile File) : IRequest<Result<StoreResult>>;