using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Spectre.Console;

namespace Presentation;

public static class ResultPrinter
{
    public static void PrintOutputPath(string name, string outputPath)
    {
        if (outputPath is null or "")
        {
            return;
        }
        
        AnsiConsole.MarkupLine($"[green]{name} saved at[/]: \"{outputPath}\"");
    }
    
    public static void PrintDescriptions(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Description.Captions is not { Count: > 0 })
        {
            return;
        }
        
        AnsiConsole.MarkupLine("[green]Descriptions[/]:");
        foreach (var caption in imageAnalysis.Description.Captions)
        {
            AnsiConsole.MarkupLine($" - {caption.Text} (Confidence: {caption.Confidence:P})");
        }
    }
    
    public static void PrintTags(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Tags is not { Count: > 0 })
        {
            return;
        }
    
        AnsiConsole.MarkupLine("[green]Tags[/]:");
        foreach (var tag in imageAnalysis.Tags)
        {
            AnsiConsole.MarkupLine($" - {tag.Name} (Confidence: {tag.Confidence:P})");
        }
    }

    public static void PrintCategories(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Categories is not { Count: > 0 })
        {
            return;
        }
        
        AnsiConsole.MarkupLine("[green]Categories[/]:");
        foreach (var category in imageAnalysis.Categories)
        {
            AnsiConsole.MarkupLine($" - {category.Name} (Confidence: {category.Score:P})");
        }
    }

    public static void PrintLandmarks(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Categories is not { Count: > 0 })
        {
            return;
        }
        
        List<LandmarksModel> landmarks = [];
        foreach (var category in imageAnalysis.Categories)
        {
            if (category.Detail?.Landmarks != null)
            {
                foreach (var landmark in category.Detail.Landmarks)
                {
                    if (landmarks.All(item => item.Name != landmark.Name))
                    {
                        landmarks.Add(landmark);
                    }
                }
            }
        
            if (landmarks is not { Count: > 0 })
            {
                return;
            }
            
            AnsiConsole.MarkupLine("[green]Landmarks[/]:");
            foreach(var landmark in landmarks)
            {
                AnsiConsole.MarkupLine($" - {landmark.Name} (Confidence: {landmark.Confidence:P})");
            }
        }
    }

    public static void PrintBrands(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Brands is not { Count: > 0 })
        {
            return;
        }
        
        AnsiConsole.MarkupLine("[green]Brands[/]:");
        foreach (var brand in imageAnalysis.Brands)
        {
            AnsiConsole.MarkupLine($" - {brand.Name} (Confidence: {brand.Confidence:P})");
        }
    }

    public static void PrintObjects(ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Objects is not { Count: > 0 })
        {
            return;
        }
        
        AnsiConsole.MarkupLine("[green]Objects[/]:");
        foreach (var detectedObject in imageAnalysis.Objects)
        {
            AnsiConsole.MarkupLine($" - {detectedObject.ObjectProperty} (Confidence: {detectedObject.Confidence:P})");
        }
    }

    public static void PrintAdultInfo (ImageAnalysis imageAnalysis)
    {
        if (imageAnalysis.Adult is null)
        {
            return;
        }
        
        if (imageAnalysis.Adult.IsAdultContent || imageAnalysis.Adult.IsGoryContent
                                               || imageAnalysis.Adult.IsRacyContent)
        {
            AnsiConsole.MarkupLine("[green]Ratings[/]:");
        }

        if (imageAnalysis.Adult.IsAdultContent)
        {
            AnsiConsole.MarkupLine("- Adult Content");
        }
    
        if (imageAnalysis.Adult.IsGoryContent)
        {
            AnsiConsole.MarkupLine("- Gory Content");
        }
    
        if (imageAnalysis.Adult.IsRacyContent)
        {
            AnsiConsole.MarkupLine("- Racy Content");
        }
    }
}