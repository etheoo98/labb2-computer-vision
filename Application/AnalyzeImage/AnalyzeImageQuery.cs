using Ardalis.Result;
using MediatR;

namespace Application.AnalyzeImage;

public sealed record AnalyzeImageQuery(
    string Source, 
    List<string> VisualFeatures,
    string OutputPath) : IRequest<Result<AnalyzeImageResponse>>;