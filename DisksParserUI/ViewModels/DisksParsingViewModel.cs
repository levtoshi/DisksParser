using BLL.Models;
using BLL.Services.DisksParsingServices;
using DisksParserUI.Commands.DisksParsing;
using DisksParserUI.Navigation.Services;
using System.ComponentModel;
using System.Windows.Input;
using DisksParserUI.Commands.BaseCommands;

namespace DisksParserUI.ViewModels
{
    public class DisksParsingViewModel : ViewModelBase
    {
        private readonly IDisksParsingService _disksParsingService;
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


        public DisksParsingViewModel(INavigationService navigationService, DisksStatistic disksStatistic, ParsingSettingsContext parsingSettingsContext)
        {
            _disksParsingStatistic = new DisksParsingStatistic();
            _disksParsingControlContext = new DisksParsingControlContext();
            MaxAmountOfFiles = disksStatistic.FilesPathes.Count;

            _disksParsingService = new DisksParsingService(disksStatistic, _disksParsingStatistic, parsingSettingsContext, _disksParsingControlContext);

            _startCommand = new StartParsingCommand(_disksParsingService);
            _restartCommand = new RestartParsingCommand(_disksParsingService, _disksParsingControlContext);

            StartOrRestartCommand = _startCommand;
            StopCommand = new StopParsingCommand(_disksParsingService, _disksParsingControlContext);
            AbortCommand = new AbortParsingCommand(_disksParsingService, _disksParsingControlContext);
            GoToNextViewCommand = new RelayCommand(
                (object? s) => navigationService.NavigateTo<ParsingResultsViewModel>(disksStatistic, _disksParsingStatistic, parsingSettingsContext),
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
            _disksParsingService.DisposeOnExit();

            _disksParsingControlContext.PropertyChanged -= OnControlContextPropertyChanged;
            _disksParsingStatistic.PropertyChanged -= OnParsingStatisticPropertyChanged;

            (_restartCommand as CommandBase).Dispose();
            (StopCommand as CommandBase).Dispose();
            (AbortCommand as CommandBase).Dispose();

            base.Dispose();
        }
    }
}