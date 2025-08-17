using BLL.Models;
using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.BaseCommands;
using System.ComponentModel;

namespace DisksParserUI.Commands.DisksParsing
{
    public class StopParsingCommand : CommandBase
    {
        private readonly IDisksParsingService _disksParsingService;
        private readonly DisksParsingControlContext _disksParsingControlContext;

        public StopParsingCommand(IDisksParsingService disksParsingService, DisksParsingControlContext disksParsingControlContext)
        {
            _disksParsingService = disksParsingService;
            _disksParsingControlContext = disksParsingControlContext;
            _disksParsingControlContext.PropertyChanged += OnControlContextPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            _disksParsingService.StopParsing();
        }

        public override bool CanExecute(object parameter)
        {
            return _disksParsingControlContext.IsStarted &&
                !_disksParsingControlContext.IsStopped &&
                !_disksParsingControlContext.IsEnded &&
                !_disksParsingControlContext.IsAborted &&
                base.CanExecute(parameter);
        }

        private void OnControlContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecutedChanged();
        }

        public override void Dispose()
        {
            _disksParsingControlContext.PropertyChanged -= OnControlContextPropertyChanged;
            base.Dispose();
        }
    }
}