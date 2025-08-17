using BLL.Models;
using BLL.Services.FilesCountingServices;
using DisksParserUI.Commands.BaseCommands;
using System.ComponentModel;

namespace DisksParserUI.Commands.FilesCounting
{
    public class FilesCountingCommand : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly IFilesCountingService _filesCountingService;

        private bool _isExecuted;
        public bool IsExecuted
        {
            get
            {
                return _isExecuted;
            }
            set
            {
                _isExecuted = value;
                OnCanExecutedChanged();
                OnPropertyChanged(nameof(IsExecuted));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public FilesCountingCommand(DisksStatistic disksStatistic)
        {
            _filesCountingService = new FilesCountingService(disksStatistic);
            IsExecuted = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(async () => await _filesCountingService.CountFilesAndFolders());
            IsExecuted = true;
        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuted && base.CanExecute(parameter);
        }

        public override void Dispose()
        {
            _filesCountingService.Dispose();
            base.Dispose();
        }
    }
}