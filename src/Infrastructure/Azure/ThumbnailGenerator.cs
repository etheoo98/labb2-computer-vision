using Infrastructure.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace Infrastructure.Azure;

public class ThumbnailGenerator(ComputerVisionClient cvClient)
{
    public async Task<Stream> GenerateThumbnail(string source, CancellationToken cancellationToken)
    {
        try
        {
            var sourceStream = await StreamFetcher.FetchStreamAsync(source, cancellationToken);
            var thumbnailStream = await cvClient.GenerateThumbnailInStreamAsync(184, 184, sourceStream, true, cancellationToken: cancellationToken);
            return thumbnailStream;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<string> SaveGeneratedThumbnail(Stream thumbnailStream, string outputPath)
    {
         try
         {
             const string thumbnailFileName = "thumbnail.jpg";
             var fullFilePath = Path.Combine(outputPath, thumbnailFileName);
             await using Stream thumbnailFile = File.Create(fullFilePath);
             await thumbnailStream.CopyToAsync(thumbnailFile);
             return fullFilePath;
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
    }
}