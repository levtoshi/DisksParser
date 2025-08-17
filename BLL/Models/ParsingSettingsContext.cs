using System.ComponentModel;

namespace BLL.Models
{
    public class ParsingSettingsContext : INotifyPropertyChanged
    {
        private DirectoryInfo _copyFolder;
        public DirectoryInfo CopyFolder
        {
            get
            {
                return _copyFolder;
            }
            set
            {
                _copyFolder = value;
                OnPropertyChanged(nameof(CopyFolder));
            }
        }

        public FileInfo BannedWordsFile { get; set; }
        public List<BannedWord> BannedWords { get; set; }
        public FileInfo MoreInfoFile { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ParsingSettingsContext()
        {
            BannedWords = new List<BannedWord>();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}