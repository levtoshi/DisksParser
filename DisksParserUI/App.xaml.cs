using BLL.InterfaceAccessors;
using BLL.Services.DisksParsingServices;
using BLL.Services.FilesCountingServices;
using BLL.Services.InitializeBannedWordsServices;
using BLL.Services.ParsingResultsServices;
using DisksParserUI.Accessors;
using DisksParserUI.Navigation.Services;
using DisksParserUI.Navigation.Stores;
using DisksParserUI.Stores;
using DisksParserUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DisksParserUI
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<DisksStatisticStore>();
                    services.AddSingleton<ParsingSettingsContextStore>();
                    services.AddSingleton<DisksParsingStatisticStore>();
                    services.AddSingleton<DisksParsingControlContextStore>();

                    services.AddSingleton<IDisksStatisticAccessor, DisksStatisticStoreAccessor>();
                    services.AddSingleton<IParsingSettingsContextAccessor, ParsingSettingsContextStoreAccessor>();
                    services.AddSingleton<IDisksParsingStatisticAccessor, DisksParsingStatisticStoreAccessor>();
                    services.AddSingleton<IDisksParsingControlContextAccessor, DisksParsingControlContextStoreAccessor>();

                    services.AddSingleton<IFilesCountingService, FilesCountingService>();
                    services.AddSingleton<IInitializeBannedWordsService, InitializeBannedWordsService>();
                    services.AddSingleton<IDisksParsingService, DisksParsingService>();
                    services.AddSingleton<IParsingResultsService, ParsingResultsService>();

                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<FilesCountingViewModel>();
                    services.AddTransient<InitializeParsingSettingsViewModel>();
                    services.AddTransient<InitializeBannedWordsViewModel>();
                    services.AddTransient<DisksParsingViewModel>();
                    services.AddTransient<ParsingResultsViewModel>();

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

                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });

                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs arg)
        {
            await _host.StartAsync();

            INavigationService<FilesCountingViewModel> navigationService = _host.Services.GetRequiredService<INavigationService<FilesCountingViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(arg);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}