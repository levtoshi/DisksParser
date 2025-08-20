using BLL.Models;
using BLL.Services.InitializeBannedWordsServices;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using DisksParserUI.ViewModels;
using System.ComponentModel;

namespace DisksParserUI.Commands.InitializeBannedWords
{
    public class FinishBannedWordsInitializationCommand : AsyncCommandBase, IDisposable
    {
        private readonly InitializeBannedWordsViewModel _initializeBannedWordsViewModel;
        private readonly IInitializeBannedWordsService _initializeBannedWordsService;
        private readonly INavigationService<InitializeParsingSettingsViewModel> _navigationService;
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public FinishBannedWordsInitializationCommand(InitializeBannedWordsViewModel initializeBannedWordsViewModel, IInitializeBannedWordsService initializeBannedWordsService, INavigationService<InitializeParsingSettingsViewModel> navigationService, ParsingSettingsContext parsingSettingsContext)
        {
            _initializeBannedWordsViewModel = initializeBannedWordsViewModel;
            _initializeBannedWordsViewModel.PropertyChanged += OnViewModelPropertyChanged;

            _initializeBannedWordsService = initializeBannedWordsService;
            _navigationService = navigationService;
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_parsingSettingsContext.BannedWords.Count == 0)
            {
                await _initializeBannedWordsService.SplitBannedWordsText(_initializeBannedWordsViewModel.BannedWordsText);
                Thread.Sleep(10);
            }
            _navigationService.Navigate();
        }

        public override bool CanExecute(object? parameter)
        {
            return _initializeBannedWordsViewModel.BannedWordsText.Length > 0 && base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InitializeBannedWordsViewModel.BannedWordsText))
            {
                OnCanExecutedChanged();
            }
        }

        public void Dispose()
        {
            _initializeBannedWordsViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }
}