using DisksParserUI.Navigation.Stores;

namespace DisksParserUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ViewModelBase _currentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
        }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModel.Dispose();
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }
    }
}