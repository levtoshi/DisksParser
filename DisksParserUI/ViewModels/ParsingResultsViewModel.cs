using BLL.Models;
using DisksParserUI.Commands.ParsingResults;
using System.Windows.Input;

namespace DisksParserUI.ViewModels
{
    public class ParsingResultsViewModel : ViewModelBase
    {
        public string DisksChecked { get; }
        public int AmountOfFoldersChecked { get; }
        public int AmountOfFilesChecked { get; }
        public int AmountOfFilesBanned { get; }
        public int AmountOfWordsHidden { get; }

        public ICommand MoreInfoCommand { get; }

        public ParsingResultsViewModel(DisksStatistic disksStatistic, DisksParsingStatistic disksParsingStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            DisksChecked = String.Join(" ", disksStatistic.DisksPathes);
            AmountOfFoldersChecked = disksStatistic.AmountOfFoldersCounted;
            AmountOfFilesChecked = disksParsingStatistic.AmountOfFilesChecked;
            AmountOfFilesBanned = disksParsingStatistic.AmountOfFilesBanned;
            AmountOfWordsHidden = disksParsingStatistic.AmountOfWordsHidden;
            MoreInfoCommand = new ShowMoreInfoCommand(disksParsingStatistic, parsingSettingsContext);
        }
    }
}