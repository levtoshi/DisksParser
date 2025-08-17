using BLL.Models;
using BLL.Services.InitializeBannedWordsServices;
using DisksParserUI.Commands.InitializeBannedWords;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using System.Windows.Input;

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

        public InitializeBannedWordsViewModel(INavigationService navigationService, DisksStatistic disksStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            IInitializeBannedWordsService initializeBannedWordsService = new InitializeBannedWordsService(parsingSettingsContext);

            BannedWordsText = (parsingSettingsContext.BannedWords.Count > 0) ? String.Join(" ", parsingSettingsContext.BannedWords.Select(w => w.Word)) : "";
            InitializeFileCommand = new InitializeFileWithBannedWordsCommand(this, initializeBannedWordsService, parsingSettingsContext);

            ContinueCommand = new FinishBannedWordsInitializationCommand(this, initializeBannedWordsService, navigationService, disksStatistic, parsingSettingsContext);

            GoToPreviousViewCommand = new RelayCommand((object? s) => navigationService.NavigateTo<InitializeParsingSettingsViewModel>(navigationService, disksStatistic, parsingSettingsContext));
        }

        public override void Dispose()
        {
            (ContinueCommand as CommandBase).Dispose();
            base.Dispose();
        }
    }
}