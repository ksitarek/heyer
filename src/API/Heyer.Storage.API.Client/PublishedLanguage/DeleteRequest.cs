using FluentResults;
using MediatR;

namespace Heyer.Storage.API.Client.PublishedLanguage;

public record DeleteRequest(string Key) : IRequest<Result>;