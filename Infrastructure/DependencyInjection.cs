using Infrastructure.Azure;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ComputerVisionClient>(_ =>
        {
            var key = configuration["Azure:CognitiveService:Ocp-Apim-Subscription-Key"];
            var endpoint = configuration["Azure:CognitiveService:URL"]!;
            var credentials = new ApiKeyServiceClientCredentials(key);
            var client = new ComputerVisionClient(credentials);
            client.Endpoint = endpoint;
            return client;
        });
        
        services.AddTransient<ImageAnalyzerService>();
        services.AddTransient<ThumbnailGenerator>();
    }
}