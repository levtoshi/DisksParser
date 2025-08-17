using BLL.Models;

namespace BLL.Services.InitializeBannedWordsServices
{
    public class InitializeBannedWordsService : IInitializeBannedWordsService
    {
        private readonly ParsingSettingsContext _parsingSettingsContext;

        public InitializeBannedWordsService(ParsingSettingsContext parsingSettingsContext)
        {
            _parsingSettingsContext = parsingSettingsContext;
        }

        public async Task ReadBannedWordsFile(string path)
        {
            _parsingSettingsContext.BannedWordsFile = new FileInfo(path);
            await SplitBannedWordsText(await Task.Run(() => File.ReadAllText(path)));
        }

        public async Task SplitBannedWordsText(string text)
        {
            _parsingSettingsContext.BannedWords = await Task.Run(() => text
                .Split(' ')
                .Where(word => word.All(Char.IsLetter))
                .Select(w => new BannedWord(w))
                .ToList());
        }
    }
}