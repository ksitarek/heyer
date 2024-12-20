using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Preserve;

public record PreserveRequest([FromRoute] string Key) : IRequest<Result>;