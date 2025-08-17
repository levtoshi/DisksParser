using DisksParserUI.Navigation.Services;
using DisksParserUI.Navigation.Stores;
using DisksParserUI.ViewModels;
using System.Windows;

namespace DisksParserUI
{
    public partial class App : Application
    {
        private readonly MainViewModel _mainViewModel;

        public App()
        {
            NavigationStore navigationStore = new NavigationStore();
            INavigationService navigationService = new NavigationService(navigationStore);
            navigationStore.CurrentViewModel = new FilesCountingViewModel(navigationService);
            _mainViewModel = new MainViewModel(navigationStore);
        }

        protected override void OnStartup(StartupEventArgs arg)
        {
            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            if (MainWindow != null)
            {
                MainWindow.Show();
            }

            base.OnStartup(arg);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mainViewModel.Dispose();
            base.OnExit(e);
        }
    }
}