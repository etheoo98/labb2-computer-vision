using Ardalis.Result;
using Infrastructure.Azure;
using MediatR;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Application.AnalyzeImage;

public class AnalyzeImageHandler(
    ImageAnalyzerService imageAnalyzerService) : IRequestHandler<AnalyzeImageQuery, Result<AnalyzeImageResponse>>
{
    public async Task<Result<AnalyzeImageResponse>> Handle(AnalyzeImageQuery request,
        CancellationToken cancellationToken)
    {
        var features = new List<VisualFeatureTypes?>();
        foreach (var featureName in request.VisualFeatures)
        {
            if (VisualFeatureMapping.Mapping.TryGetValue(featureName, out var visualFeature))
            {
                features.Add(visualFeature);
            }
        }
        
        var imageAnalysis = await imageAnalyzerService.AnalyzeImage(request.Source, features, cancellationToken);
        var analysisOutputPath = await ImageAnalyzerService.SaveImageAnalysis(imageAnalysis, request.OutputPath, cancellationToken);

        var objectsOutputPath = string.Empty;
        if (imageAnalysis.Objects.Count >= 1)
        {
            objectsOutputPath = await ImageAnalyzerService.SaveImageAnalysisObjects(imageAnalysis, request.Source, request.OutputPath, cancellationToken);
        }
        
        return Result<AnalyzeImageResponse>.Success(new AnalyzeImageResponse(imageAnalysis, analysisOutputPath, objectsOutputPath));
    }
}