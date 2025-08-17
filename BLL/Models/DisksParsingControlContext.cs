using System.ComponentModel;

namespace BLL.Models
{
    public class DisksParsingControlContext : INotifyPropertyChanged
    {
        private CancellationTokenSource _controlContextCancellationTokenSource;
        public CancellationTokenSource ControlContextCancellationTokenSource
        {
            get
            {
                return _controlContextCancellationTokenSource;
            }
        }

        private CancellationToken _controlContextCancellationToken;
        public CancellationToken ControlContextCancellationToken
        {
            get
            {
                return _controlContextCancellationToken;
            }
        }

        private SemaphoreSlim _threadsControlSlim;
        public SemaphoreSlim ThreadsControlSlim
        {
            get
            {
                return _threadsControlSlim;
            }
        }

        private ManualResetEventSlim _stopHandle;
        public ManualResetEventSlim StopHandle
        {
            get
            {
                return _stopHandle;
            }
        }

        public int MaxConcurrency { get; }

        private bool _isStarted;
        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
            set
            {
                _isStarted = value;
                OnPropertyChanged(nameof(IsStarted));
            }
        }

        private bool _isStopped;
        public bool IsStopped
        {
            get
            {
                return _isStopped;
            }
            set
            {
                _isStopped = value;
                OnPropertyChanged(nameof(IsStopped));
            }
        }

        private bool _isEnded;
        public bool IsEnded
        {
            get
            {
                return _isEnded;
            }
            set
            {
                _isEnded = value;
                OnPropertyChanged(nameof(IsEnded));
            }
        }

        private bool _isAborted;
        public bool IsAborted
        {
            get
            {
                return _isAborted;
            }
            set
            {
                _isAborted = value;
                OnPropertyChanged(nameof(IsAborted));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DisksParsingControlContext()
        {
            MaxConcurrency = 20;
            
            IsStarted = false;
            IsStopped = false;
            IsEnded = false;
            IsAborted = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeControlContextProperties()
        {
            _controlContextCancellationTokenSource = new CancellationTokenSource();
            _controlContextCancellationToken = _controlContextCancellationTokenSource.Token;
            _threadsControlSlim = new SemaphoreSlim(MaxConcurrency);
            _stopHandle = new ManualResetEventSlim(true);
        }
    }
}