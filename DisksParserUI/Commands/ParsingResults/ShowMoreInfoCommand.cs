using BLL.Models;
using BLL.Services.ParsingResultsServices;
using DisksParserUI.Commands.BaseCommands;

namespace DisksParserUI.Commands.ParsingResults
{
    public class ShowMoreInfoCommand : AsyncCommandBase
    {
        private readonly IParsingResultsService _parsingResultsService;
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public ShowMoreInfoCommand(DisksParsingStatistic disksParsingStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            _parsingResultsService = new ParsingResultsService(disksParsingStatistic, parsingSettingsContext);
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_parsingSettingsContext.MoreInfoFile == null)
            {
                await _parsingResultsService.FormMoreInfoFile();
            }
            await _parsingResultsService.OpenMoreInfoFile();
        }
    }
}