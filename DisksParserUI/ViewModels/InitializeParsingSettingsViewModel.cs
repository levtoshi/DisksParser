using BLL.Models;
using DisksParserUI.Commands.InitializeParsingSettings;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using System.ComponentModel;
using System.Windows.Input;
using DisksParserUI.Stores;

namespace DisksParserUI.ViewModels
{
    public class InitializeParsingSettingsViewModel : ViewModelBase
    {
        private readonly ParsingSettingsContext _parsingSettingsContext;

        private string? _pathToFileWithBannedWords => _parsingSettingsContext.BannedWordsFile?.FullName;
        public string? PathToFileWithBannedWords
        {
            get
            {
                return _pathToFileWithBannedWords;
            }
        }

        private string? _copyFolderPath => _parsingSettingsContext.CopyFolder?.FullName;
        public string? CopyFolderPath
        {
            get
            {
                return _copyFolderPath;
            }
        }

        public ICommand InitializeWordsCommand { get; }
        public ICommand InitializeFolderCommand { get; }
        public ICommand GoToNextViewCommand { get; }


        public InitializeParsingSettingsViewModel(INavigationService<DisksParsingViewModel> navigateToDisksParsingService, INavigationService<InitializeBannedWordsViewModel> navigateToBannedWordsService, ParsingSettingsContextStore parsingSettingsContextStore)
        {
            _parsingSettingsContext = parsingSettingsContextStore.ParsingSettingsContextObject;
            InitializeWordsCommand = new InitializeBannedWordsCommand(navigateToBannedWordsService);
            InitializeFolderCommand = new InitializeCopyFolderCommand(_parsingSettingsContext);

            GoToNextViewCommand = new RelayCommand(
                (object? s) => navigateToDisksParsingService.Navigate(),
                (object? s) => _parsingSettingsContext.BannedWords.Count > 0 && _parsingSettingsContext.CopyFolder != null);

            _parsingSettingsContext.PropertyChanged += OnParsingSettingsPropertyChanged;
        }

        private void OnParsingSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParsingSettingsContext.CopyFolder))
            {
                OnPropertyChanged(nameof(CopyFolderPath));
                (GoToNextViewCommand as RelayCommand)?.OnCanExecutedChanged();
            }
        }

        public override void Dispose()
        {
            _parsingSettingsContext.PropertyChanged -= OnParsingSettingsPropertyChanged;
            base.Dispose();
        }
    }
}