using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Application.AnalyzeImage;

public sealed record AnalyzeImageResponse(
    ImageAnalysis ImageAnalysis,
    string AnalysisOutputPath,
    string ObjectsOutputPath);