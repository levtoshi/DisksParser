using BLL.Models;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using DisksParserUI.ViewModels;

namespace DisksParserUI.Commands.InitializeParsingSettings
{
    public class InitializeBannedWordsCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        private readonly DisksStatistic _disksStatistic;
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public InitializeBannedWordsCommand(INavigationService navigationService, DisksStatistic disksStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            _navigationService = navigationService;
            _disksStatistic = disksStatistic;
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateTo<InitializeBannedWordsViewModel>(_navigationService, _disksStatistic, _parsingSettingsContext);
        }
    }
}