using Ardalis.Result;
using MediatR;

namespace Application.CreateThumbnail;

public sealed record CreateThumbnailQuery(
    string Source,
    string OutputPath) : IRequest<Result<CreateThumbnailResponse>>;