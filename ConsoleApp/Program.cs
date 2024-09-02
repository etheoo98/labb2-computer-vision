using Application;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation;

var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
{
    services.AddApplication();
    services.AddInfrastructure(context.Configuration);
    services.AddTransient<PresentationRunner>();
}).ConfigureLogging(logging =>
{
    logging.ClearProviders();
}).Build();

var presentationRunner = host.Services.GetRequiredService<PresentationRunner>();
await presentationRunner.Run();