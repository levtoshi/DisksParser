using BLL.Models;
using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.DisksParsing;
using DisksParserUI.Navigation.Services;
using System.ComponentModel;
using System.Windows.Input;
using DisksParserUI.Commands.BaseCommands;
using DisksParserUI.Stores;

namespace DisksParserUI.ViewModels
{
    public class DisksParsingViewModel : ViewModelBase
    {
        private readonly DisksParsingStatistic _disksParsingStatistic;
        private readonly DisksParsingControlContext _disksParsingControlContext;

        private int _amountOfSecondsParsing => _disksParsingStatistic.AmountOfSecondsParsing;
        public int AmountOfSecondsParsing
        {
            get
            {
                return _amountOfSecondsParsing;
            }
        }

        private int _amountOfFilesChecked => _disksParsingStatistic.AmountOfFilesChecked;
        public int AmountOfFilesChecked
        {
            get
            {
                return _amountOfFilesChecked;
            }
        }

        public int MaxAmountOfFiles { get; }

        private int _amountOfFilesBanned => _disksParsingStatistic.AmountOfFilesBanned;
        public int AmountOfFilesBanned
        {
            get
            {
                return _amountOfFilesBanned;
            }
        }

        private int _amountOfWordsHidden => _disksParsingStatistic.AmountOfWordsHidden;
        public int AmountOfWordsHidden
        {
            get
            {
                return _amountOfWordsHidden;
            }
        }

        private bool _isStarted => _disksParsingControlContext.IsStarted;
        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
        }

        private readonly ICommand _startCommand;
        private readonly ICommand _restartCommand;
        public ICommand StartOrRestartCommand { get; set; }

        public ICommand StopCommand { get; }
        public ICommand AbortCommand { get; }
        public ICommand GoToNextViewCommand { get; }


        public DisksParsingViewModel(INavigationService<ParsingResultsViewModel> navigationService, IDisksParsingService disksParsingService, DisksStatisticStore disksStatisticStore, DisksParsingStatisticStore disksParsingStatisticStore, DisksParsingControlContextStore disksParsingControlContextStore)
        {
            _disksParsingStatistic = disksParsingStatisticStore.DisksParsingStatisticObject;
            _disksParsingControlContext = disksParsingControlContextStore.DisksParsingControlContextObject;
            MaxAmountOfFiles = disksStatisticStore.DisksStatisticObject.FilesPathes.Count;

            _startCommand = new StartParsingCommand(disksParsingService);
            _restartCommand = new RestartParsingCommand(disksParsingService, _disksParsingControlContext);

            StartOrRestartCommand = _startCommand;
            StopCommand = new StopParsingCommand(disksParsingService, _disksParsingControlContext);
            AbortCommand = new AbortParsingCommand(disksParsingService, _disksParsingControlContext);
            GoToNextViewCommand = new RelayCommand(
                (object? s) => navigationService.Navigate(),
                (object? s) => _disksParsingControlContext.IsEnded);

            _disksParsingControlContext.PropertyChanged += OnControlContextPropertyChanged;
            _disksParsingStatistic.PropertyChanged += OnParsingStatisticPropertyChanged;
        }

        private void OnControlContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisksParsingControlContext.IsStarted))
            {
                StartOrRestartCommand = _disksParsingControlContext.IsStarted ? _restartCommand : _startCommand;
                OnPropertyChanged(nameof(StartOrRestartCommand));
                OnPropertyChanged(nameof(IsStarted));
            }
            if (e.PropertyName == nameof(DisksParsingControlContext.IsEnded))
            {
                (GoToNextViewCommand as RelayCommand).OnCanExecutedChanged();
            }
        }

        private void OnParsingStatisticPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DisksParsingStatistic.AmountOfSecondsParsing))
            {
                OnPropertyChanged(nameof(AmountOfSecondsParsing));
            }
            if (e.PropertyName == nameof(DisksParsingStatistic.AmountOfFilesChecked))
            {
                OnPropertyChanged(nameof(AmountOfFilesChecked));
            }
            if (e.PropertyName == nameof(DisksParsingStatistic.AmountOfFilesBanned))
            {
                OnPropertyChanged(nameof(AmountOfFilesBanned));
            }
            if (e.PropertyName == nameof(DisksParsingStatistic.AmountOfWordsHidden))
            {
                OnPropertyChanged(nameof(AmountOfWordsHidden));
            }
        }

        public override void Dispose()
        {
            _disksParsingControlContext.PropertyChanged -= OnControlContextPropertyChanged;
            _disksParsingStatistic.PropertyChanged -= OnParsingStatisticPropertyChanged;

            if (_restartCommand is IDisposable disposable1)
            {
                disposable1.Dispose();
            }
            if (StopCommand is IDisposable disposable2)
            {
                disposable2.Dispose();
            }
            if (AbortCommand is IDisposable disposable3)
            {
                disposable3.Dispose();
            }

            base.Dispose();
        }
    }
}