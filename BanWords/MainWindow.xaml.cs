using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BLL.Services;

namespace BanWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BanWordsService banWordsService;
        private InitializeBannedWordsWind initializeBannedWordsWind;

        private Timer timerBanning;
        private int secondsBanning;

        private Timer timerCounting;
        private int secondsCounting;

        private readonly string abortQuestion = "Are you sure? All progress and banned files will be lost!";

        public MainWindow()
        {
            InitializeComponent();
            InitializeService();
        }

        private void InitializeService()
        {
            banWordsService = new BanWordsService();
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private async Task UpdateCounted()
        {
            while (banWordsService.CountingFiles)
            {
                await Task.Delay(250);
                UpdateCountBody();
            }
            await Task.Delay(1000);
            UpdateCountBody();
        }

        private void UpdateCountBody()
        {
            Dispatcher.Invoke(() =>
            {
                totalFoldersTb.Text = $"Total folders counted: {banWordsService.AmountOfFolders}";
                totalFilesTb.Text = $"Total text files counted: {banWordsService.FilesPathes.Count}";
            });
        }

        private async void countFilesBt_Click(object sender, RoutedEventArgs e)
        {
            timerCounting = new Timer(TimerCountingMethod);
            timerCounting.Change(0, 1000);

            countFilesBt.IsEnabled = false;
            banWordsService.CountingFiles = true;

            Task.Run(async () => await UpdateCounted());
            await Task.Run(async () => await banWordsService.CountFilesAndFolders());

            timerCounting.Dispose();

            initializeBannedBt.IsEnabled = true;
            selectFolderForBannedBt.IsEnabled = true;
            mainProgressBar.Maximum = banWordsService.FilesPathes.Count;
        }

        private void TimerCountingMethod(object obj)
        {
            Dispatcher.Invoke(() => timerCountFilesTb.Text =
                $"{secondsCounting / 60:D2}:{secondsCounting % 60:D2}");
            secondsCounting++;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private async void initializeBannedBt_Click(object sender, RoutedEventArgs e)
        {
            await RunInitializeBannedWind();
            CheckFileSearchStart();
        }

        private async Task RunInitializeBannedWind()
        {
            initializeBannedWordsWind = new InitializeBannedWordsWind();
            initializeBannedWordsWind.ShowDialog();

            if (initializeBannedWordsWind.IsReady.Count(a => a) == 1)
            {
                if (initializeBannedWordsWind.IsReady[0])
                {
                    await banWordsService.ReadBannedWordsFile(initializeBannedWordsWind.BannedWordsFile.FullName);
                }
                else
                {
                    await banWordsService.SplitBannedWordsText(initializeBannedWordsWind.Words);
                }
            }
        }

        private void selectFolderForBannedBt_Click(object sender, RoutedEventArgs e)
        {
            banWordsService.SelectCopyFolder();
            CheckFileSearchStart();
        }

        private void CheckFileSearchStart()
        {
            if (banWordsService.SelectButtonsStates.All(s => s))
            {
                startBt.IsEnabled = true;
            }
            UpdatePathesTextBlock();
        }

        private void UpdatePathesTextBlock()
        {
            pathesTb.Text = "";
            if (banWordsService.CopyFolder != null)
            {
                pathesTb.Text += $"Path to copy folder: {banWordsService.CopyFolder.FullName}\n";
            }
            if (banWordsService.BannedWordsFile != null)
            {
                pathesTb.Text += $"Path to banned words file: {banWordsService.BannedWordsFile.FullName}\n";
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private async Task UpdateDynamicAnalysis()
        {
            while (banWordsService.BanningFiles)
            {
                await Task.Delay(250);
                UpdateBanBody();
            }
            await Task.Delay(1000);
            UpdateBanBody();
        }

        private void UpdateBanBody()
        {
            Dispatcher.Invoke(() =>
            {
                filesCheckedTb.Text = $"Total files checked: {banWordsService.AmountOfFilesChecked}";
                filesBannedTb.Text = $"Total files banned: {banWordsService.AmountOFFilesBanned}";
                wordsHiddenTb.Text = $"Total words hidden: {banWordsService.AmountOfWordsHidden}";
                mainProgressBar.Value = banWordsService.AmountOfFilesChecked;
            });
        }

        private void UpdateNotDynamicAnalysis()
        {
            disksCheckedTb.Text = $"Disks checked: {string.Join(" ", banWordsService.DisksPathes)}";
            foldersCheckedTb.Text = $"Total folders checked: {banWordsService.AmountOfFolders}";
        }

        private async void startBt_Click(object sender, RoutedEventArgs e)
        {
            timerBanning = new Timer(TimerBanningMethod);
            timerBanning.Change(0, 1000);

            banWordsService.CancellationTokenSource = new CancellationTokenSource();
            banWordsService.CancellationToken = banWordsService.CancellationTokenSource.Token;

            this.startBt.Click -= startBt_Click;
            this.startBt.Click += RestartProcess;

            initializeBannedBt.IsEnabled = false;
            selectFolderForBannedBt.IsEnabled = false;
            startBt.IsEnabled = false;
            stopBt.IsEnabled = true;
            abortBt.IsEnabled = true;

            banWordsService.BanningFiles = true;

            Task.Run(async () => await UpdateDynamicAnalysis());
            await Task.Run(async () => await banWordsService.MainAsyncCore());

            if(banWordsService.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            timerBanning.Dispose();

            moreInfoBt.IsEnabled = true;
            stopBt.IsEnabled = false;
            abortBt.IsEnabled = false;

            UpdateNotDynamicAnalysis();
        }

        private void TimerBanningMethod(object obj)
        {
            Dispatcher.Invoke(() => timerTb.Text =
                $"{secondsBanning / 60:D2}:{secondsBanning % 60:D2}");
            secondsBanning++;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void stopBt_Click(object sender, RoutedEventArgs e)
        {
            banWordsService.StopHandle.Reset();

            this.stopBt.IsEnabled = false;
            this.startBt.Content = "Restart";
            this.startBt.IsEnabled = true;

            timerBanning.Dispose();
        }

        private async void abortBt_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(abortQuestion, "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                timerBanning.Dispose();

                this.startBt.IsEnabled = false;
                this.stopBt.IsEnabled = false;
                this.abortBt.IsEnabled = false;

                banWordsService.CancellationTokenSource.Cancel();
                banWordsService.CancellationTokenSource.Dispose();

                if (!banWordsService.StopHandle.IsSet)
                {
                    banWordsService.StopHandle.Set();
                }

                await banWordsService.AbortProcessMethod();

                secondsBanning = 0;
                this.mainProgressBar.Value = 0;

                UpdateBanBody();
                TimerBanningMethod(0);

                this.startBt.Click -= RestartProcess;
                this.startBt.Click += startBt_Click;
                this.startBt.Content = "Start";
                this.startBt.IsEnabled = true;
            }
        }

        private void RestartProcess(object sender, RoutedEventArgs e)
        {
            banWordsService.StopHandle.Set();
            timerBanning = new Timer(TimerBanningMethod);
            timerBanning.Change(0, 1000);

            this.stopBt.IsEnabled = true;
            this.startBt.IsEnabled = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private async void moreInfoBt_Click(object sender, RoutedEventArgs e)
        {
            await banWordsService.OpenMoreInfoFile();
        }
    }
}