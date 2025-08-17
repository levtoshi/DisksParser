using System.ComponentModel;

namespace BLL.Models
{
    public class BannedWord
    {
        public string Word { get; }
        public int Count { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BannedWord(string word)
        {
            Word = word;
        }
    }
}