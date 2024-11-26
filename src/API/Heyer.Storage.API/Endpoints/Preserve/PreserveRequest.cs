using FluentResults;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Preserve;

public record PreserveRequest(string Key) : IRequest<Result>;