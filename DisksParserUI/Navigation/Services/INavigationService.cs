using DisksParserUI.ViewModels;

namespace DisksParserUI.Navigation.Services
{
    public interface INavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        void Navigate();
    }
}