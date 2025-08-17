using BLL.Models;
using BLL.Services.InitializeBannedWordsServices;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.ViewModels;
using System.Windows.Forms;

namespace DisksParserUI.Commands.InitializeBannedWords
{
    public class InitializeFileWithBannedWordsCommand : AsyncCommandBase
    {
        private readonly InitializeBannedWordsViewModel _initializeBannedWordsViewModel;
        private readonly IInitializeBannedWordsService _initializeBannedWordsService;
        private readonly ParsingSettingsContext _parsingSettingsContext;
        private readonly string _fileFilter = "Text file (*.txt)|*.txt";

        public InitializeFileWithBannedWordsCommand(InitializeBannedWordsViewModel initializeBannedWordsViewModel, IInitializeBannedWordsService initializeBannedWordsService, ParsingSettingsContext parsingSettingsContext)
        {
            _initializeBannedWordsViewModel = initializeBannedWordsViewModel;
            _initializeBannedWordsService = initializeBannedWordsService;
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _fileFilter;
            openFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog.FileName))
            {
                await _initializeBannedWordsService.ReadBannedWordsFile(openFileDialog.FileName);
                _initializeBannedWordsViewModel.BannedWordsText = String.Join(" ", _parsingSettingsContext.BannedWords.Select(s => s.Word));
            }
        }
    }
}