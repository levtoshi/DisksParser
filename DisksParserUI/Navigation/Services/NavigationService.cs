using DisksParserUI.Navigation.Stores;
using DisksParserUI.ViewModels;

namespace DisksParserUI.Navigation.Services
{
    public class NavigationService : INavigationService
    {
        private readonly NavigationStore _navigationStore;

        public NavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase
        {
            _navigationStore.CurrentViewModel.Dispose();

            ViewModelBase viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), parameters);

            if (viewModel != null)
            {
                _navigationStore.CurrentViewModel = viewModel;
            }
        }
    }
}