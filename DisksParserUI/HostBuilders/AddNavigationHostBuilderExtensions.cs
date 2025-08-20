using DisksParserUI.Navigation.Services;
using DisksParserUI.Navigation.Stores;
using DisksParserUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisksParserUI.HostBuilders
{
    public static class AddNavigationHostBuilderExtensions
    {
        public static IHostBuilder AddNavigation(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore>();

                services.AddSingleton<Func<FilesCountingViewModel>>((s) => () => s.GetRequiredService<FilesCountingViewModel>());
                services.AddSingleton<INavigationService<FilesCountingViewModel>, NavigationService<FilesCountingViewModel>>();

                services.AddSingleton<Func<InitializeParsingSettingsViewModel>>((s) => () => s.GetRequiredService<InitializeParsingSettingsViewModel>());
                services.AddSingleton<INavigationService<InitializeParsingSettingsViewModel>, NavigationService<InitializeParsingSettingsViewModel>>();

                services.AddSingleton<Func<InitializeBannedWordsViewModel>>((s) => () => s.GetRequiredService<InitializeBannedWordsViewModel>());
                services.AddSingleton<INavigationService<InitializeBannedWordsViewModel>, NavigationService<InitializeBannedWordsViewModel>>();

                services.AddSingleton<Func<DisksParsingViewModel>>((s) => () => s.GetRequiredService<DisksParsingViewModel>());
                services.AddSingleton<INavigationService<DisksParsingViewModel>, NavigationService<DisksParsingViewModel>>();

                services.AddSingleton<Func<ParsingResultsViewModel>>((s) => () => s.GetRequiredService<ParsingResultsViewModel>());
                services.AddSingleton<INavigationService<ParsingResultsViewModel>, NavigationService<ParsingResultsViewModel>>();
            });

            return hostBuilder;
        }
    }
}