using BLL.Models;
using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.BaseCommands;
using System.ComponentModel;

namespace DisksParserUI.Commands.DisksParsing
{
    public class RestartParsingCommand : CommandBase, IDisposable
    {
        private readonly IDisksParsingService _disksParsingService;
        private readonly DisksParsingControlContext _disksParsingControlContext;

        public RestartParsingCommand(IDisksParsingService disksParsingService, DisksParsingControlContext disksParsingControlContext)
        {
            _disksParsingService = disksParsingService;
            _disksParsingControlContext = disksParsingControlContext;
            _disksParsingControlContext.PropertyChanged += OnControlContextPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            _disksParsingService.RestartParsing();
        }

        public override bool CanExecute(object parameter)
        {
            return _disksParsingControlContext.IsStopped &&
                !_disksParsingControlContext.IsAborted &&
                base.CanExecute(parameter);
        }

        private void OnControlContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisksParsingControlContext.IsStopped) ||
                e.PropertyName == nameof(DisksParsingControlContext.IsAborted))
            {
                OnCanExecutedChanged();
            }
        }

        public void Dispose()
        {
            _disksParsingControlContext.PropertyChanged -= OnControlContextPropertyChanged;
        }
    }
}