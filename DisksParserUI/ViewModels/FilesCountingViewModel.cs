using BLL.Models;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Commands.FilesCounting;
using DisksParserUI.Navigation.Services;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;


namespace DisksParserUI.ViewModels
{
    public class FilesCountingViewModel : ViewModelBase
    {
        private readonly DisksStatistic _disksStatistic;

        private int _amountOfSecondsExecuting => _disksStatistic.AmountOfSecondsExecuting;
        public int AmountOfSecondsExecuting
        {
            get
            {
                return _amountOfSecondsExecuting;
            }
        }

        private int _totalFoldersCounted => _disksStatistic.AmountOfFoldersCounted;
        public int TotalFoldersCounted
        {
            get
            {
                return _totalFoldersCounted;
            }
        }

        private int _totalFilesCounted => _disksStatistic.FilesPathes.Count;
        public int TotalFilesCounted
        {
            get
            {
                return _totalFilesCounted;
            }
        }

        public FilesCountingCommand CountingCommand { get; }
        public ICommand GoToNextViewCommand { get; }

        public FilesCountingViewModel(INavigationService navigationService)
        {
            _disksStatistic = new DisksStatistic();
            _disksStatistic.PropertyChanged += OnDisksStatisticPropertyChanged;
            _disksStatistic.FilesPathes.CollectionChanged += OnFilePathesChanged;

            CountingCommand = new FilesCountingCommand(_disksStatistic);

            GoToNextViewCommand = new RelayCommand(
                (object? p) => navigationService.NavigateTo<InitializeParsingSettingsViewModel>(navigationService, _disksStatistic, new ParsingSettingsContext()),
                (object? p) => CountingCommand.IsExecuted);

            CountingCommand.PropertyChanged += OnCountingCommandPropertyChanged;
        }



        private void OnDisksStatisticPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisksStatistic.AmountOfSecondsExecuting))
            {
                OnPropertyChanged(nameof(AmountOfSecondsExecuting));
            }
            if (e.PropertyName == nameof(DisksStatistic.AmountOfFoldersCounted))
            {
                OnPropertyChanged(nameof(TotalFoldersCounted));
            }
        }

        private void OnFilePathesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalFilesCounted));
        }

        private void OnCountingCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CountingCommand.IsExecuted))
            {
                (GoToNextViewCommand as RelayCommand)?.OnCanExecutedChanged();
            }
        }

        public override void Dispose()
        {
            _disksStatistic.PropertyChanged -= OnDisksStatisticPropertyChanged;
            _disksStatistic.FilesPathes.CollectionChanged -= OnFilePathesChanged;

            CountingCommand.PropertyChanged -= OnCountingCommandPropertyChanged;
            CountingCommand.Dispose();

            base.Dispose();
        }
    }
}