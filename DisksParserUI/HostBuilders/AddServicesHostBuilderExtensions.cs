using BLL.Services.DisksParsingServices;
using BLL.Services.FilesCountingServices;
using BLL.Services.InitializeBannedWordsServices;
using BLL.Services.ParsingResultsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisksParserUI.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IFilesCountingService, FilesCountingService>();
                services.AddSingleton<IInitializeBannedWordsService, InitializeBannedWordsService>();
                services.AddSingleton<IDisksParsingService, DisksParsingService>();
                services.AddSingleton<IParsingResultsService, ParsingResultsService>();
            });

            return hostBuilder;
        }
    }
}