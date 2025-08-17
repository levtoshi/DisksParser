using BLL.Models;
using System.Text.RegularExpressions;

namespace BLL.Services.DisksParsingServices
{
    public class DisksParsingService : IDisksParsingService
    {
        private readonly DisksStatistic _disksStatistic;
        private readonly DisksParsingStatistic _disksParsingStatistic;
        private readonly ParsingSettingsContext _parsingSettingsContext;
        private readonly DisksParsingControlContext _disksParsingControlContext;

        private readonly string _firstFolderName = "BanWords";
        private readonly string _copyFolderName = "BannedFiles";
        private readonly string _bannedWordBlure = "*******";

        private Timer _timerCounting;

        public DisksParsingService(DisksStatistic disksStatistic, DisksParsingStatistic disksParsingStatistic, ParsingSettingsContext parsingSettingsContext, DisksParsingControlContext disksParsingControlContext)
        {
            _disksStatistic = disksStatistic;
            _disksParsingStatistic = disksParsingStatistic;
            _parsingSettingsContext = parsingSettingsContext;
            _disksParsingControlContext = disksParsingControlContext;
        }

        public async Task StartParsing()
        {
            _disksParsingControlContext.InitializeControlContextProperties();

            _timerCounting = new Timer(_disksParsingStatistic.IncrementAmountOfSecondsParsing);
            _timerCounting.Change(0, 1000);

            CreateCopyArchitecture();
            _disksParsingControlContext.IsStarted = true;
            await BanAsyncCore();

            _timerCounting.Dispose();
        }

        private void CreateCopyArchitecture()
        {
            string basePath = Path.Combine(_parsingSettingsContext.CopyFolder.FullName, _firstFolderName);
            string targetFolder = Path.Combine(basePath, _copyFolderName);

            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(targetFolder);

            _parsingSettingsContext.CopyFolder = new DirectoryInfo(targetFolder);
        }


        private async Task BanAsyncCore()
        {
            List<Task> tasks = new List<Task>();

            foreach (string filePath in _disksStatistic.FilesPathes)
            {
                _disksParsingControlContext.StopHandle.Wait();

                if (_disksParsingControlContext.ControlContextCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await _disksParsingControlContext.ThreadsControlSlim.WaitAsync();

                Task task = Task.Run(() =>
                {
                    try
                    {
                        BanFileAsync(filePath);
                    }
                    catch { }
                    finally
                    {
                        _disksParsingControlContext.ThreadsControlSlim.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            _disksParsingControlContext.IsEnded = true;
        }

        private string TryReadFileAsync(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void BanFileAsync(string path)
        {
            try
            {
                string text = TryReadFileAsync(path);
                if (String.IsNullOrWhiteSpace(text))
                    return;

                int wordsBefore = _disksParsingStatistic.AmountOfWordsHidden;

                if (AnalyseFile(text))
                {
                    string copyPath = GenerateCopyPath(path);

                    BlureFileAsync(copyPath, text);

                    lock (this)
                    {
                        _disksParsingStatistic.BannedFilesInfo.Add(new BannedFileInfo
                        {
                            Path = path,
                            AmountOfBans = (_disksParsingStatistic.AmountOfWordsHidden - wordsBefore),
                            Size = new FileInfo(path).Length
                        });
                    }
                    _disksParsingStatistic.IncrementAmountOfFilesBanned();
                }
            }
            catch { }
            finally
            {
                _disksParsingStatistic.IncrementAmountOfFilesChecked();
            }
        }

        private bool AnalyseFile(string text)
        {
            bool banned = false;
            foreach (BannedWord word in _parsingSettingsContext.BannedWords)
            {
                int count = Regex.Matches(text, Regex.Escape(word.Word)).Count;
                if (count > 0)
                {
                    banned = true;
                    word.Count += count;
                    _disksParsingStatistic.IncreaseAmountOfWordsHidden(count);
                }
            }
            return banned;
        }

        private string GenerateCopyPath(string originalPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(originalPath);
            string ext = Path.GetExtension(originalPath);
            string basePath = _parsingSettingsContext.CopyFolder.FullName;

            int i = 1;
            string newPath;
            do
            {
                newPath = Path.Combine(basePath, $"{fileName}_{i++}{ext}");
            } while (File.Exists(newPath));

            if (_disksParsingControlContext.ControlContextCancellationToken.IsCancellationRequested)
            {
                return "";
            }

            File.Copy(originalPath, newPath);

            return newPath;
        }

        private void BlureFileAsync(string copyPath, string text)
        {
            foreach (BannedWord word in _parsingSettingsContext.BannedWords)
            {
                text = text.Replace(word.Word, _bannedWordBlure);
            }
            try
            {
                File.WriteAllText(copyPath, text);
            }
            catch { }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void StopParsing()
        {
            _disksParsingControlContext.IsStopped = true;
            _timerCounting.Dispose();
            _disksParsingControlContext.StopHandle.Reset();
        }

        public void RestartParsing()
        {
            _disksParsingControlContext.IsStopped = false;
            _timerCounting.Change(0, 1000);
            _disksParsingControlContext.StopHandle.Set();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task Dispose()
        {
            _disksParsingControlContext.IsAborted = true;

            _timerCounting.Dispose();
            _disksParsingControlContext.ControlContextCancellationTokenSource.Cancel();
            _disksParsingControlContext.ControlContextCancellationTokenSource.Dispose();

            if (!_disksParsingControlContext.StopHandle.IsSet)
            {
                _disksParsingControlContext.StopHandle.Set();
            }

            await Task.Run(() => DeleteFolder());

            _disksParsingStatistic.AllToZero();

            foreach (BannedWord bannedWord in _parsingSettingsContext.BannedWords)
            {
                bannedWord.Count = 0;
            }
            DisposeEventHandlers();

            _disksParsingControlContext.IsAborted = false;
            _disksParsingControlContext.IsStopped = false;
            _disksParsingControlContext.IsStarted = false;
        }

        public void DisposeOnExit()
        {
            _timerCounting.Dispose();

            try
            {
                _disksParsingControlContext.ControlContextCancellationTokenSource.Cancel();
                _disksParsingControlContext.ControlContextCancellationTokenSource.Dispose();
            }
            catch { }

            if (!_disksParsingControlContext.StopHandle.IsSet)
            {
                _disksParsingControlContext.StopHandle.Set();
            }
            DisposeEventHandlers();
        }

        private void DeleteFolder()
        {
            DirectoryInfo ParentFolder = new DirectoryInfo(Directory.GetParent(_parsingSettingsContext.CopyFolder.FullName).FullName);

            Thread.Sleep(1000);

            foreach (FileInfo item in _parsingSettingsContext.CopyFolder.GetFiles())
            {
                item.Delete();
            }

            _parsingSettingsContext.CopyFolder.Delete();

            _parsingSettingsContext.CopyFolder = new DirectoryInfo(Directory.GetParent(ParentFolder.FullName).FullName);

            ParentFolder.Delete();
        }

        private void DisposeEventHandlers()
        {
            _disksParsingControlContext.StopHandle.Dispose();
            _disksParsingControlContext.ThreadsControlSlim.Dispose();
        }
    }
}