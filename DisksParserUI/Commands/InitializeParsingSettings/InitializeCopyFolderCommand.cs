using BLL.Models;
using DisksParserUI.Commands.BaseCommands;
using Ookii.Dialogs.Wpf;
using System.IO;

namespace DisksParserUI.Commands.InitializeParsingSettings
{
    public class InitializeCopyFolderCommand : CommandBase
    {
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public InitializeCopyFolderCommand(ParsingSettingsContext parsingSettingsContext)
        {
            _parsingSettingsContext = parsingSettingsContext;
        }

        public override void Execute(object? parameter)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Select folder",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == true)
            {
                _parsingSettingsContext.CopyFolder = new DirectoryInfo(dialog.SelectedPath);
            }
        }
    }
}