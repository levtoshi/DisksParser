using BLL.Models;
using BLL.Services.InitializeBannedWordsServices;
using DisksParserUI.Commands.InitializeBannedWords;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using System.Windows.Input;
using DisksParserUI.Stores;

namespace DisksParserUI.ViewModels
{
    public class InitializeBannedWordsViewModel : ViewModelBase
    {
        private string _bannedWordsText;
        public string BannedWordsText
        {
            get
            {
                return _bannedWordsText;
            }
            set
            {
                _bannedWordsText = value;
                OnPropertyChanged(nameof(BannedWordsText));
            }
        }

        public ICommand InitializeFileCommand { get; }
        public ICommand ContinueCommand { get; }
        public ICommand GoToPreviousViewCommand { get; }

        public InitializeBannedWordsViewModel(INavigationService<InitializeParsingSettingsViewModel> navigationService, IInitializeBannedWordsService initializeBannedWordsService, ParsingSettingsContextStore parsingSettingsContextStore)
        {
            ParsingSettingsContext parsingSettingsContext = parsingSettingsContextStore.ParsingSettingsContextObject;

            BannedWordsText = (parsingSettingsContext.BannedWords.Count > 0) ? String.Join(" ", parsingSettingsContext.BannedWords.Select(w => w.Word)) : "";
            InitializeFileCommand = new InitializeFileWithBannedWordsCommand(this, initializeBannedWordsService, parsingSettingsContext);

            ContinueCommand = new FinishBannedWordsInitializationCommand(this, initializeBannedWordsService, navigationService, parsingSettingsContext);

            GoToPreviousViewCommand = new RelayCommand((object? s) => navigationService.Navigate());
        }

        public override void Dispose()
        {
            if (ContinueCommand is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.Dispose();
        }
    }
}