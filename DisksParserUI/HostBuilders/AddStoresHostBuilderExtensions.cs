using DisksParserUI.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisksParserUI.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<DisksStatisticStore>();
                services.AddSingleton<ParsingSettingsContextStore>();
                services.AddSingleton<DisksParsingStatisticStore>();
                services.AddSingleton<DisksParsingControlContextStore>();
            });

            return hostBuilder;
        }
    }
}