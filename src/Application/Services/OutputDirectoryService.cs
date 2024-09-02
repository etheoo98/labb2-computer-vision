using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class OutputDirectoryService(IConfiguration configuration)
{
    public string CreateOutputDirectory()
    {
        string outputPath;

        if (string.IsNullOrEmpty(configuration["OutputPath"]))
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            outputPath = Path.Combine(documentPath, "ComputerVision");
        }
        else
        {
            outputPath = configuration["OutputPath"]!;
        }

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        var fullPath = Path.Combine(outputPath, $"Analysis-{DateTime.Now:yyyy.MM.dd-HH.mm.ss.ff}");
        Directory.CreateDirectory(fullPath);
        return fullPath;
    }
}