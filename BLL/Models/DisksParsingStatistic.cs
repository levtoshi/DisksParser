using System.ComponentModel;

namespace BLL.Models
{
    public class DisksParsingStatistic : INotifyPropertyChanged
    {
        private int _amountOfSecondsParsing;
        public int AmountOfSecondsParsing
        {
            get
            {
                return _amountOfSecondsParsing;
            }
        }

        private int _amountOfFilesChecked;
        public int AmountOfFilesChecked
        {
            get
            {
                return _amountOfFilesChecked;
            }
        }

        private int _amountOFFilesBanned;
        public int AmountOfFilesBanned
        {
            get
            {
                return _amountOFFilesBanned;
            }
        }
        private int _amountOfWordsHidden;
        public int AmountOfWordsHidden
        {
            get
            {
                return _amountOfWordsHidden;
            }
        }

        private readonly List<BannedFileInfo> _bannedFilesInfo;
        public List<BannedFileInfo> BannedFilesInfo
        {
            get
            {
                return _bannedFilesInfo;
            }
        }

        public DisksParsingStatistic()
        {
            _bannedFilesInfo = new List<BannedFileInfo>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncrementAmountOfSecondsParsing(object? obj)
        {
            Interlocked.Increment(ref _amountOfSecondsParsing);
            OnPropertyChanged(nameof(AmountOfSecondsParsing));
        }

        public void IncrementAmountOfFilesChecked()
        {
            Interlocked.Increment(ref _amountOfFilesChecked);
            OnPropertyChanged(nameof(AmountOfFilesChecked));
        }

        public void IncrementAmountOfFilesBanned()
        {
            Interlocked.Increment(ref _amountOFFilesBanned);
            OnPropertyChanged(nameof(AmountOfFilesBanned));
        }

        public void IncreaseAmountOfWordsHidden(int value)
        {
            Interlocked.Add(ref _amountOfWordsHidden, value);
            OnPropertyChanged(nameof(AmountOfWordsHidden));
        }

        public void AllToZero()
        {
            _amountOfSecondsParsing = 0;
            _amountOfFilesChecked = 0;
            _amountOfWordsHidden = 0;
            _amountOFFilesBanned = 0;
            OnPropertyChanged(nameof(AmountOfSecondsParsing));
            OnPropertyChanged(nameof(AmountOfFilesChecked));
            OnPropertyChanged(nameof(AmountOfFilesBanned));
            OnPropertyChanged(nameof(AmountOfWordsHidden));
        }
    }
}