using BLL.Models;
using BLL.Services.ParsingResultsServices;
using DisksParserUI.Commands.ParsingResults;
using DisksParserUI.Stores;
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

        public ParsingResultsViewModel(IParsingResultsService parsingResultsService, DisksStatisticStore disksStatisticStore, DisksParsingStatisticStore disksParsingStatisticStore, ParsingSettingsContextStore parsingSettingsContextStore)
        {
            DisksChecked = String.Join(" ", disksStatisticStore.DisksStatisticObject.DisksPathes);
            AmountOfFoldersChecked = disksStatisticStore.DisksStatisticObject.AmountOfFoldersCounted;
            AmountOfFilesChecked = disksParsingStatisticStore.DisksParsingStatisticObject.AmountOfFilesChecked;
            AmountOfFilesBanned = disksParsingStatisticStore.DisksParsingStatisticObject.AmountOfFilesBanned;
            AmountOfWordsHidden = disksParsingStatisticStore.DisksParsingStatisticObject.AmountOfWordsHidden;
            MoreInfoCommand = new ShowMoreInfoCommand(parsingResultsService, parsingSettingsContextStore.ParsingSettingsContextObject);
        }
    }
}