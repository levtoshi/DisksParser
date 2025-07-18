using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace BanWords
{
    /// <summary>
    /// Interaction logic for InitializeBannedWordsWind.xaml
    /// </summary>
    public partial class InitializeBannedWordsWind : Window
    {
        private OpenFileDialog openFileDialog;
        private readonly string fileFilter = "Text file (*.txt)|*.txt";

        public FileInfo BannedWordsFile { get; set; }
        public string Words { get; set; }

        public List<bool> IsReady { get; set; }

        public InitializeBannedWordsWind()
        {
            InitializeComponent();
            InitializeStates();
        }

        private void InitializeStates()
        {
            IsReady = new List<bool>();
            IsReady.Add(false);
            IsReady.Add(false);
        }

        private void selectFileWithBannedWordsBt_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = fileFilter;
            openFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog.FileName))
            {
                BannedWordsFile = new FileInfo(openFileDialog.FileName);
                IsReady[0] = true;
                this.Close();
            }
        }

        private void continueBt_Click(object sender, RoutedEventArgs e)
        {
            if(IsValid())
            {
                Words = this.wordsTb.Text;
                IsReady[1] = true;
                this.Close();
            }
        }

        private void ClearBorders()
        {
            this.wordsTb.BorderBrush = Brushes.LightCyan;
        }

        private bool IsValid()
        {
            ClearBorders();
            if(String.IsNullOrEmpty(this.wordsTb.Text) || String.IsNullOrWhiteSpace(this.wordsTb.Text))
            {
                this.wordsTb.BorderBrush = Brushes.Red;
                return false;
            }
            return true;
        }
    }
}