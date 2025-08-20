using BLL.InterfaceAccessors;
using DisksParserUI.Accessors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DisksParserUI.HostBuilders
{
    public static class AddAccessorsHostBuilderExtensions
    {
        public static IHostBuilder AddAccessors(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IDisksStatisticAccessor, DisksStatisticStoreAccessor>();
                services.AddSingleton<IParsingSettingsContextAccessor, ParsingSettingsContextStoreAccessor>();
                services.AddSingleton<IDisksParsingStatisticAccessor, DisksParsingStatisticStoreAccessor>();
                services.AddSingleton<IDisksParsingControlContextAccessor, DisksParsingControlContextStoreAccessor>();
            });

            return hostBuilder;
        }
    }
}