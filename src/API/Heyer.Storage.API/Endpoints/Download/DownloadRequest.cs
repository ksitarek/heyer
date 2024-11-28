using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Endpoints.Download;

public record DownloadRequest([FromRoute(Name = "Key")]string Key) : IRequest<Result<DownloadResponse>>;