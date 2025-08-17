using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.BaseCommands;

namespace DisksParserUI.Commands.DisksParsing
{
    public class StartParsingCommand : AsyncCommandBase
    {
        private readonly IDisksParsingService _disksParsingService;

        public StartParsingCommand(IDisksParsingService disksParsingService)
        {
            _disksParsingService = disksParsingService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(async () => await _disksParsingService.StartParsing());
        }
    }
}