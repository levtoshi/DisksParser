using BLL.Models;
using BLL.Services.InitializeBannedWordsServices;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using DisksParserUI.ViewModels;
using System.ComponentModel;

namespace DisksParserUI.Commands.InitializeBannedWords
{
    public class FinishBannedWordsInitializationCommand : AsyncCommandBase
    {
        private readonly InitializeBannedWordsViewModel _initializeBannedWordsViewModel;
        private readonly IInitializeBannedWordsService _initializeBannedWordsService;
        private readonly INavigationService _navigationService;
        private readonly DisksStatistic _disksStatistic;
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public FinishBannedWordsInitializationCommand(InitializeBannedWordsViewModel initializeBannedWordsViewModel, IInitializeBannedWordsService initializeBannedWordsService, INavigationService navigationService, DisksStatistic disksStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            _initializeBannedWordsViewModel = initializeBannedWordsViewModel;
            _initializeBannedWordsViewModel.PropertyChanged += OnViewModelPropertyChanged;

            _initializeBannedWordsService = initializeBannedWordsService;
            _navigationService = navigationService;
            _disksStatistic = disksStatistic;
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_parsingSettingsContext.BannedWords.Count == 0)
            {
                await _initializeBannedWordsService.SplitBannedWordsText(_initializeBannedWordsViewModel.BannedWordsText);
                Thread.Sleep(10);
            }
            _navigationService.NavigateTo<InitializeParsingSettingsViewModel>(_navigationService, _disksStatistic, _parsingSettingsContext);
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

        public override void Dispose()
        {
            _initializeBannedWordsViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            base.Dispose();
        }
    }
}