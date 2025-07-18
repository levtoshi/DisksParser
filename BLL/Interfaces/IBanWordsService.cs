using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBanWordsService
    {
        int AmountOfFilesChecked { get; set; }
        int AmountOFFilesBanned { get; set; }
        int AmountOfWordsHidden { get; set; }
        int AmountOfFolders { get; set; }

        List<string> FilesPathes { get; set; }
        List<string> DisksPathes { get; set; }
        List<bool> SelectButtonsStates { get; set; }

        DirectoryInfo CopyFolder { get; set; }
        FileInfo BannedWordsFile { get; set; }

        bool CountingFiles { get; set; }
        bool BanningFiles { get; set; }

        CancellationTokenSource CancellationTokenSource { get; set; }
        CancellationToken CancellationToken { get; set; }

        SemaphoreSlim Semaphore { get; set; }
        ManualResetEventSlim StopHandle { get; set; }


        void SelectCopyFolder();
        Task ReadBannedWordsFile(string path);
        Task SplitBannedWordsText(string text);
        Task CountFilesAndFolders();
        Task MainAsyncCore();
        Task AbortProcessMethod();
        Task OpenMoreInfoFile();
    }
}