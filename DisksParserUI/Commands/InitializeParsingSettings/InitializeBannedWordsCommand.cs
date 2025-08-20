using BLL.Models;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Navigation.Services;
using DisksParserUI.ViewModels;

namespace DisksParserUI.Commands.InitializeParsingSettings
{
    public class InitializeBannedWordsCommand : CommandBase
    {
        private readonly INavigationService<InitializeBannedWordsViewModel> _navigateToBannedWordsService;

        public InitializeBannedWordsCommand(INavigationService<InitializeBannedWordsViewModel> navigateToBannedWordsService)
        {
            _navigateToBannedWordsService = navigateToBannedWordsService;
        }

        public override void Execute(object? parameter)
        {
            _navigateToBannedWordsService.Navigate();
        }
    }
}