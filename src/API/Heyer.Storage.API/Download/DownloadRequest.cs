using FluentResults;
using Heyer.Storage.API.Client.PublishedLanguage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Heyer.Storage.API.Download;

public record DownloadRequest([FromRoute(Name = "Key")] string Key) : IRequest<Result<DownloadResponse>>;