using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Client.PublishedLanguage;

public record PreserveRequest([FromRoute] string Key) : IRequest<Result>;