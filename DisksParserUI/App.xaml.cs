using DisksParserUI.Navigation.Services;
using DisksParserUI.ViewModels;
using DisksParserUI.HostBuilders;
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
                .AddStores()
                .AddAccessors()
                .AddServices()
                .AddViewModels()
                .AddNavigation()
                .ConfigureServices((context, services) =>
                {
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