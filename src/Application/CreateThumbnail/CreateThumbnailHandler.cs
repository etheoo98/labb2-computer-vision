using Ardalis.Result;
using Infrastructure.Azure;
using MediatR;

namespace Application.CreateThumbnail;

public class CreateThumbnailHandler(
    ThumbnailGenerator thumbnailGenerator) : IRequestHandler<CreateThumbnailQuery, Result<CreateThumbnailResponse>>
{
    public async Task<Result<CreateThumbnailResponse>> Handle(CreateThumbnailQuery request,
        CancellationToken cancellationToken)
    { 
        var thumbnailStream = await thumbnailGenerator.GenerateThumbnail(request.Source, cancellationToken);
        var outputPath = await ThumbnailGenerator.SaveGeneratedThumbnail(thumbnailStream, request.OutputPath);
        return Result<CreateThumbnailResponse>.Success(new CreateThumbnailResponse(thumbnailStream, outputPath));
    }
}