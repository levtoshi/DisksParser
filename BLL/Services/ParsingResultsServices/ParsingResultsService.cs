using BLL.InterfaceAccessors;
using BLL.Models;
using System.Diagnostics;
using System.Text;

namespace BLL.Services.ParsingResultsServices
{
    public class ParsingResultsService : IParsingResultsService
    {
        private readonly DisksParsingStatistic _disksParsingStatistic;
        private readonly ParsingSettingsContext _parsingSettingsContext;

        private readonly string _detailsFileName = "Details";

        public ParsingResultsService(IDisksParsingStatisticAccessor disksParsingStatisticAccessor, IParsingSettingsContextAccessor parsingSettingsContextAccessor)
        {
            _disksParsingStatistic = disksParsingStatisticAccessor.GetDisksParsingStatistic();
            _parsingSettingsContext = parsingSettingsContextAccessor.GetParsingSettingsContext();
        }

        public async Task FormMoreInfoFile()
        {
            await CreateDetailsFile();
            await WriteToDetailsFile();
        }

        private async Task CreateDetailsFile()
        {
            string basePath = Directory.GetParent(_parsingSettingsContext.CopyFolder.FullName).FullName;
            string detailsPath = Path.Combine(basePath, $"{_detailsFileName}.txt");
            await Task.Run(() => File.Create(detailsPath).Close());

            _parsingSettingsContext.MoreInfoFile = new FileInfo(detailsPath);
        }

        private async Task WriteToDetailsFile()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("--------------------BANNED FILES--------------------\n");
            int i = 1;
            foreach (BannedFileInfo file in _disksParsingStatistic.BannedFilesInfo)
            {
                sb.AppendLine($"{i++}.----------------------------------------------------");
                sb.AppendLine($"PATH: {file.Path}");
                sb.AppendLine($"SIZE: {file.Size}");
                sb.AppendLine($"AMOUNT OF BANS: {file.AmountOfBans}");
                sb.AppendLine("----------------------------------------------------\n");
            }

            sb.AppendLine("--------------------TOP 10 BANNED WORDS--------------------\n");
            List<BannedWord> topWords = _parsingSettingsContext.BannedWords.OrderByDescending(w => w.Count).Take(10).ToList();
            for (int j = 0; j < topWords.Count; j++)
            {
                sb.AppendLine($"{j + 1}. {topWords[j].Word} : {topWords[j].Count}");
            }

            await Task.Run(() => File.WriteAllText(_parsingSettingsContext.MoreInfoFile.FullName, sb.ToString()));
        }

        public async Task OpenMoreInfoFile()
        {
            await Task.Run(() => Process.Start("notepad.exe", _parsingSettingsContext.MoreInfoFile.FullName));
        }
    }
}