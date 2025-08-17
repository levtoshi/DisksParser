using BLL.Models;

namespace BLL.Services.FilesCountingServices
{
    public class FilesCountingService : IFilesCountingService
    {
        private readonly DisksStatistic _disksStatistic;
        private readonly Timer _timerCounting;

        private readonly CancellationTokenSource _disposeCancellationTokenSource;
        private readonly CancellationToken _disposeCancellationToken;

        public FilesCountingService(DisksStatistic disksStatistic)
        {
            _disksStatistic = disksStatistic;
            _timerCounting = new Timer(_disksStatistic.IncrementAmountOfSecondsExecuting);
            _disposeCancellationTokenSource = new CancellationTokenSource();
            _disposeCancellationToken = _disposeCancellationTokenSource.Token;
        }

        public async Task CountFilesAndFolders()
        {
            _timerCounting.Change(0, 1000);
            _disksStatistic.DisksPathes = DriveInfo.GetDrives().Select(d => d.Name).ToList();
            List<Task> tasks = _disksStatistic.DisksPathes.Select(path => Task.Run(() => SearchFolders(path))).ToList();
            await Task.WhenAll(tasks);
            _timerCounting.Dispose();
        }

        private void SearchFolders(string path)
        {
            if (_disposeCancellationToken.IsCancellationRequested)
            {
                return;
            }
            try
            {
                List<DirectoryInfo> dirs = new DirectoryInfo(path).GetDirectories().ToList();
                SearchFiles(path);
                _disksStatistic.IncrementAmountOfFoldersCounted();
                Parallel.ForEach(dirs, dir => SearchFolders(dir.FullName));
            }
            catch { }
        }

        private void SearchFiles(string path)
        {
            if (_disposeCancellationToken.IsCancellationRequested)
            {
                return;
            }
            try
            {
                string[] allowedExtensions = new string[] { ".txt", ".log", ".csv", ".cpp", ".c", ".cs", ".py", ".js" };

                List<FileInfo> files = new DirectoryInfo(path)
                    .GetFiles()
                    .Where(f => allowedExtensions.Contains(f.Extension.ToLower()))
                    .ToList();

                //List<FileInfo> files = new DirectoryInfo(path).GetFiles("*.txt").ToList();
                foreach (FileInfo file in files)
                {
                    lock (this)
                    {
                        _disksStatistic.FilesPathes.Add(file.FullName);
                    }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            _timerCounting.Dispose();
            _disposeCancellationTokenSource.Cancel();
            _disposeCancellationTokenSource.Dispose();
        }
    }
}