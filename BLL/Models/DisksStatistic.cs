using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BLL.Models
{
    public class DisksStatistic : INotifyPropertyChanged
    {
        private int _amountOfSecondsExecuting;
        public int AmountOfSecondsExecuting
        {
            get
            {
                return _amountOfSecondsExecuting;
            }
        }

        private int _amountOfFoldersCounted;
        public int AmountOfFoldersCounted
        {
            get
            {
                return _amountOfFoldersCounted;
            }
        }

        public ObservableCollection<string> FilesPathes { get; }
        public List<string> DisksPathes { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DisksStatistic()
        {
            FilesPathes = new ObservableCollection<string>();
            DisksPathes = new List<string>();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void IncrementAmountOfSecondsExecuting(object? obj)
        {
            Interlocked.Increment(ref _amountOfSecondsExecuting);
            OnPropertyChanged(nameof(AmountOfSecondsExecuting));
        }

        public void IncrementAmountOfFoldersCounted()
        {
            Interlocked.Increment(ref _amountOfFoldersCounted);
            OnPropertyChanged(nameof(AmountOfFoldersCounted));
        }
    }
}