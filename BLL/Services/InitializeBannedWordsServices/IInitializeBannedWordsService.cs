namespace BLL.Services.InitializeBannedWordsServices
{
    public interface IInitializeBannedWordsService
    {
        Task ReadBannedWordsFile(string path);
        Task SplitBannedWordsText(string text);
    }
}