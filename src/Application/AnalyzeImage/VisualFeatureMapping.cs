using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Application.AnalyzeImage;

public static class VisualFeatureMapping
{
    public static readonly Dictionary<string, VisualFeatureTypes> Mapping = new()
    {
        { "Faces", VisualFeatureTypes.Faces },
        { "Description", VisualFeatureTypes.Description },
        { "Tags", VisualFeatureTypes.Tags },
        { "Categories", VisualFeatureTypes.Categories },
        { "Brands", VisualFeatureTypes.Brands },
        { "Objects", VisualFeatureTypes.Objects },
        { "Adult", VisualFeatureTypes.Adult }
    };

    public static IReadOnlyList<string> GetFeatureNames() => Mapping.Keys.ToList();
}