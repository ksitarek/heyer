using FluentResults;
using MediatR;

namespace Heyer.Storage.API.CleanupTempFiles;

public record CleanupTempFilesRequest() : IRequest<Result>;