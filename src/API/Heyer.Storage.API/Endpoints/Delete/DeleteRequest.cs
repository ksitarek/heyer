using FluentResults;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Delete;

public record DeleteRequest(string Key) : IRequest<Result>;