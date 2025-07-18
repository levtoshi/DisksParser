using BLL.Interfaces;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace BLL.Services
{
    public class BanWordsService : IBanWordsService
    {
        private int _amountOfFilesChecked;
        public int AmountOfFilesChecked
        {
            get => _amountOfFilesChecked;
            set => _amountOfFilesChecked = value;
        }

        private int _amountOFFilesBanned;
        public int AmountOFFilesBanned
        {
            get => _amountOFFilesBanned;
            set => _amountOFFilesBanned = value;
        }

        private int _amountOfWordsHidden;
        public int AmountOfWordsHidden
        {
            get => _amountOfWordsHidden;
            set => _amountOfWordsHidden = value;
        }

        private int _amountOfFolders;
        public int AmountOfFolders
        {
            get => _amountOfFolders;
            set => _amountOfFolders = value;
        }

        public List<string> FilesPathes { get; set; } = new List<string>();
        public List<string> DisksPathes { get; set; } = new List<string>();
        public List<bool> SelectButtonsStates { get; set; } = new List<bool>() { false, false };

        public DirectoryInfo CopyFolder { get; set; }
        public FileInfo BannedWordsFile { get; set; }
        private FileInfo _moreInfoFile;

        private readonly string _firstFolderName = "BanWords";
        private readonly string _copyFolderName = "BannedFiles";
        private readonly string _detailsFileName = "Details";
        private readonly string _bannedWordBlure = "*******";

        private List<BannedWord> _bannedWords = new List<BannedWord>();
        private List<BannedFileInfo> _bannedFilesInfo = new List<BannedFileInfo>();

        public bool CountingFiles { get; set; }
        public bool BanningFiles { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public SemaphoreSlim Semaphore { get; set; }
        public ManualResetEventSlim StopHandle { get; set; } = new ManualResetEventSlim(true);
        private int _maxConcurrency = 20;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SelectCopyFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.SelectedPath))
            {
                CopyFolder = new DirectoryInfo(dialog.SelectedPath);
                SelectButtonsStates[1] = true;
            }
        }

        public async Task ReadBannedWordsFile(string path)
        {
            BannedWordsFile = new FileInfo(path);
            await SplitBannedWordsText(await Task.Run(() => File.ReadAllText(path)));
        }

        public async Task SplitBannedWordsText(string text)
        {
            _bannedWords = await Task.Run(() => text
                .Split(' ')
                .Where(word => word.All(char.IsLetter))
                .Select(w => new BannedWord { Word = w, Count = 0 })
                .ToList());
            SelectButtonsStates[0] = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task CountFilesAndFolders()
        {
            DisksPathes = DriveInfo.GetDrives().Select(d => d.Name).ToList();
            List<Task> tasks = DisksPathes.Select(path => Task.Run(() => SearchFolders(path))).ToList();
            await Task.WhenAll(tasks);
            CountingFiles = false;
        }

        private void SearchFolders(string path)
        {
            try
            {
                List<DirectoryInfo> dirs = new DirectoryInfo(path).GetDirectories().ToList();
                SearchFiles(path);
                Interlocked.Increment(ref _amountOfFolders);
                Parallel.ForEach(dirs, dir => SearchFolders(dir.FullName));
            }
            catch { }
        }

        private void SearchFiles(string path)
        {
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
                        FilesPathes.Add(file.FullName);
                    }
                }
            }
            catch { }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task MainAsyncCore()
        {
            CreateCopyArchitecture();
            await BanAsyncCore();
            BanningFiles = false;
        }

        private void CreateCopyArchitecture()
        {
            string basePath = Path.Combine(CopyFolder.FullName, _firstFolderName);
            string targetFolder = Path.Combine(basePath, _copyFolderName);
            string detailsPath = Path.Combine(basePath, $"{_detailsFileName}.txt");

            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(targetFolder);
            File.Create(detailsPath).Close();

            CopyFolder = new DirectoryInfo(targetFolder);
            _moreInfoFile = new FileInfo(detailsPath);
        }
        

        private async Task BanAsyncCore()
        {
            Semaphore = new SemaphoreSlim(_maxConcurrency);
            List<Task> tasks = new List<Task>();

            foreach (string filePath in FilesPathes)
            {
                StopHandle.Wait();

                if (CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Semaphore.WaitAsync();

                Task task = Task.Run(() =>
                {
                    try
                    {
                        BanFileAsync(filePath);
                    }
                    catch { }
                    finally
                    {
                        Semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await FormMoreInfoFile();
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

                int wordsBefore = AmountOfWordsHidden;

                if (AnalyseFile(text))
                {
                    string copyPath = GenerateCopyPath(path);

                    BlureFileAsync(copyPath, text);
                    
                    lock(this)
                    {
                        _bannedFilesInfo.Add(new BannedFileInfo
                        {
                            Path = path,
                            AmountOfBans = (AmountOfWordsHidden - wordsBefore),
                            Size = new FileInfo(path).Length
                        });
                    }

                    Interlocked.Increment(ref _amountOFFilesBanned);
                }
            }
            catch { }
            finally
            {
                Interlocked.Increment(ref _amountOfFilesChecked);
            }
        }

        private bool AnalyseFile(string text)
        {
            bool banned = false;
            foreach (BannedWord word in _bannedWords)
            {
                int count = Regex.Matches(text, Regex.Escape(word.Word)).Count;
                if (count > 0)
                {
                    banned = true;
                    word.Count += count;
                    Interlocked.Add(ref _amountOfWordsHidden, count);
                }
            }
            return banned;
        }

        private string GenerateCopyPath(string originalPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(originalPath);
            string ext = Path.GetExtension(originalPath);
            string basePath = CopyFolder.FullName;

            int i = 1;
            string newPath;
            do
            {
                newPath = Path.Combine(basePath, $"{fileName}_{i++}{ext}");
            } while (File.Exists(newPath));

            if (CancellationToken.IsCancellationRequested)
            {
                return "";
            }

            File.Copy(originalPath, newPath);

            return newPath;
        }

        private void BlureFileAsync(string copyPath, string text)
        {
            foreach (BannedWord word in _bannedWords)
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

        private async Task FormMoreInfoFile()
        {
            if (CancellationToken.IsCancellationRequested)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("--------------------BANNED FILES--------------------\n");
            int i = 1;
            foreach (BannedFileInfo file in _bannedFilesInfo)
            {
                sb.AppendLine($"{i++}.----------------------------------------------------");
                sb.AppendLine($"PATH: {file.Path}");
                sb.AppendLine($"SIZE: {file.Size}");
                sb.AppendLine($"AMOUNT OF BANS: {file.AmountOfBans}");
                sb.AppendLine("----------------------------------------------------\n");
            }

            sb.AppendLine("--------------------TOP 10 BANNED WORDS--------------------\n");
            List<BannedWord> topWords = _bannedWords.OrderByDescending(w => w.Count).Take(10).ToList();
            for (int j = 0; j < topWords.Count; j++)
            {
                sb.AppendLine($"{j + 1}. {topWords[j].Word} : {topWords[j].Count}");
            }

            await Task.Run(() => File.WriteAllText(_moreInfoFile.FullName, sb.ToString()));
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task AbortProcessMethod()
        {
            await Task.Run(() => DeleteFolder());

            AmountOFFilesBanned = 0;
            AmountOfFilesChecked = 0;
            AmountOfWordsHidden = 0;

            foreach (BannedWord bannedWord in _bannedWords)
            {
                bannedWord.Count = 0;
            }
            BanningFiles = false;
        }

        private async Task DeleteFolder()
        {
            DirectoryInfo ParentFolder = new DirectoryInfo(Directory.GetParent(CopyFolder.FullName).FullName);

            _moreInfoFile.Delete();

            await Task.Delay(1000);

            foreach (FileInfo item in CopyFolder.GetFiles())
            {
                item.Delete();
            }

            CopyFolder.Delete();
            
            CopyFolder = new DirectoryInfo(Directory.GetParent(ParentFolder.FullName).FullName);
            
            ParentFolder.Delete();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task OpenMoreInfoFile()
        {
            await Task.Run(() => Process.Start("notepad.exe", _moreInfoFile.FullName));
        }
    }
}