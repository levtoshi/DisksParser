using DisksParserUI.ViewModels;

namespace DisksParserUI.Navigation.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase;
    }
}