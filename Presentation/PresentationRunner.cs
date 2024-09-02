using Application.AnalyzeImage;
using Application.CreateThumbnail;
using Application.Services;
using MediatR;
using Spectre.Console;

namespace Presentation;

public class PresentationRunner(
    OutputDirectoryService outputDirectoryService,
    ISender sender)
{
    public async Task Run()
    {
        while (true)
        {
            var userInput = AnsiConsole.Prompt(new TextPrompt<string>("[deepskyblue1]Enter image path or URL[/]:"));
            if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")
            {
                break;
            }
            
            var visualFeatures = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Choose [green]Visual Features[/] of interest")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a choice, " + 
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(VisualFeatureMapping.GetFeatureNames()));
        
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("springgreen3"))
                .StartAsync("Loading",
                    async ctx =>
                    {
                        ctx.Status("Creating output directory");
                        var outputPath = outputDirectoryService.CreateOutputDirectory();
                    
                        ctx.Status("Analyzing image");
                        var analyzeImageResponse = await SendAnalyzeImageRequest(userInput, visualFeatures, outputPath);
                    
                        ctx.Status("Generating Thumbnail");
                        var thumbnailResponse = await SendCreateThumbnailRequest(userInput, outputPath);
                    
                        PrintResults(analyzeImageResponse, thumbnailResponse);
                    });
        }
    }
    
    private async Task<AnalyzeImageResponse> SendAnalyzeImageRequest(string source, List<string> visualFeatures, string outputPath)
    {
        var request = new AnalyzeImageQuery(source, visualFeatures, outputPath);
        var result = await sender.Send(request);
        return result;
    }
    
    private async Task<CreateThumbnailResponse> SendCreateThumbnailRequest(string source, string outputPath)
    {
        var request = new CreateThumbnailQuery(source, outputPath);
        var result = await sender.Send(request);
        return result;
    }

    private static void PrintResults(AnalyzeImageResponse analyzeImageResponse, CreateThumbnailResponse thumbnailResponse)
    {
        ResultPrinter.PrintOutputPath("Analysis", analyzeImageResponse.AnalysisOutputPath);
        ResultPrinter.PrintOutputPath("Objects", analyzeImageResponse.ObjectsOutputPath);
        ResultPrinter.PrintOutputPath("Thumbnail", thumbnailResponse.OutputPath);
        ResultPrinter.PrintDescriptions(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintBrands(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintCategories(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintLandmarks(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintObjects(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintTags(analyzeImageResponse.ImageAnalysis);
        ResultPrinter.PrintAdultInfo(analyzeImageResponse.ImageAnalysis);
        AnsiConsole.WriteLine();
    }
}