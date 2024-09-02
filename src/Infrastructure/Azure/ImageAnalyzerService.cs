using System.Text.Json;
using Infrastructure.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using SkiaSharp;

namespace Infrastructure.Azure;

public class ImageAnalyzerService(
    ComputerVisionClient cvClient)
{
    public async Task<ImageAnalysis> AnalyzeImage(string source, List<VisualFeatureTypes?> features, CancellationToken cancellationToken)
    {
        try
        {
            var imageStream = await StreamFetcher.FetchStreamAsync(source, cancellationToken);
            var imageAnalysis = await cvClient.AnalyzeImageInStreamAsync(imageStream, features, cancellationToken: cancellationToken); 
            return imageAnalysis;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<string> SaveImageAnalysis(ImageAnalysis imageAnalysis, string outputPath, CancellationToken cancellationToken)
    {
        try
        {
            var options = new JsonSerializerOptions
            { 
                WriteIndented = true
            };

            var imageAnalysisJson = JsonSerializer.Serialize(imageAnalysis, options);
            var fullFilePath = Path.Combine(outputPath, "ImageAnalysis.json");
            await File.WriteAllTextAsync(fullFilePath, imageAnalysisJson, cancellationToken);
            return fullFilePath;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<string> SaveImageAnalysisObjects(ImageAnalysis imageAnalysis, string source, string outputPath, CancellationToken cancellationToken)
    {
        try
        {
            var imageStream = await StreamFetcher.FetchStreamAsync(source, cancellationToken);
        
            using var original = SKBitmap.Decode(imageStream);
            using var image = new SKBitmap(original.Width, original.Height);
            var textSize = Math.Max(image.Width, image.Height) / 50f;

            using var canvas = new SKCanvas(image);
            canvas.DrawBitmap(original, 0, 0);

            using var penPaint = new SKPaint();
            penPaint.Color = SKColors.Red;
            penPaint.StrokeWidth = 3;
            penPaint.IsStroke = true;

            using var textPaint = new SKPaint();
            textPaint.Color = SKColors.White;
            textPaint.TextSize = textSize;
            textPaint.IsAntialias = true;
            textPaint.Style = SKPaintStyle.Fill;

            using var outlinePaint = new SKPaint();
            outlinePaint.Color = SKColors.Black;
            outlinePaint.TextSize = textSize;
            outlinePaint.IsAntialias = true;
            outlinePaint.Style = SKPaintStyle.Stroke;
            outlinePaint.StrokeWidth = 3;

            foreach (var detectedObject in imageAnalysis.Objects)
            {
                var r = detectedObject.Rectangle;
                var rect = new SKRect(r.X, r.Y, r.X + r.W, r.Y + r.H);

                canvas.DrawRect(rect, penPaint);
                canvas.DrawText(detectedObject.ObjectProperty, r.X, r.Y + textSize, outlinePaint);
                canvas.DrawText(detectedObject.ObjectProperty, r.X, r.Y + textSize, textPaint);
            }

            var fullFilePath = Path.Combine(outputPath, "objects.jpg");
            await using var outputStream = File.OpenWrite(fullFilePath);
            image.Encode(outputStream, SKEncodedImageFormat.Jpeg, 75);
            return fullFilePath;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}