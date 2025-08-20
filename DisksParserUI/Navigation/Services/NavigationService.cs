using DisksParserUI.Navigation.Stores;
using DisksParserUI.ViewModels;

namespace DisksParserUI.Navigation.Services
{
    public class NavigationService<TViewModel> : INavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            if (_navigationStore.CurrentViewModel != null)
            {
                _navigationStore.CurrentViewModel.Dispose();
            }
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}