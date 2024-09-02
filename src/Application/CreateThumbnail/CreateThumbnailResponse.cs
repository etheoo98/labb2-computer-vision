namespace Application.CreateThumbnail;

public sealed record CreateThumbnailResponse(
    Stream ThumbnailStream,
    string OutputPath);