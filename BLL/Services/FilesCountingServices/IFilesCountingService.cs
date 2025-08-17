namespace BLL.Services.FilesCountingServices
{
    public interface IFilesCountingService
    {
        Task CountFilesAndFolders();
        void Dispose();
    }
}