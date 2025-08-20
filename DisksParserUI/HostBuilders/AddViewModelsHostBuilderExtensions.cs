using DisksParserUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisksParserUI.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddTransient<FilesCountingViewModel>();
                services.AddTransient<InitializeParsingSettingsViewModel>();
                services.AddTransient<InitializeBannedWordsViewModel>();
                services.AddTransient<DisksParsingViewModel>();
                services.AddTransient<ParsingResultsViewModel>();
            });

            return hostBuilder;
        }
    }
}