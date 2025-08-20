using BLL.Models;
using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.BaseCommands;
using System.ComponentModel;
using System.Windows;

namespace DisksParserUI.Commands.DisksParsing
{
    public class AbortParsingCommand : AsyncCommandBase, IDisposable
    {
        private readonly IDisksParsingService _disksParsingService;
        private readonly DisksParsingControlContext _disksParsingControlContext;
        private readonly string _abortQuestion = "Are you sure? All progress and banned files will be lost!";

        public AbortParsingCommand(IDisksParsingService disksParsingService, DisksParsingControlContext disksParsingControlContext)
        {
            _disksParsingService = disksParsingService;
            _disksParsingControlContext = disksParsingControlContext;
            _disksParsingControlContext.PropertyChanged += OnControlContextPropertyChanged;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (MessageBox.Show(_abortQuestion, "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _disksParsingService.DisposeOnAbort();
            }
        }
        public override bool CanExecute(object parameter)
        {
            return _disksParsingControlContext.IsStarted &&
                !_disksParsingControlContext.IsEnded &&
                base.CanExecute(parameter);
        }

        private void OnControlContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisksParsingControlContext.IsStarted) ||
                e.PropertyName == nameof(DisksParsingControlContext.IsEnded))
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