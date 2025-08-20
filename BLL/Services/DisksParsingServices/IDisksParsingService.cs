namespace BLL.Services.DisksParsingServices
{
    public interface IDisksParsingService
    {
        Task StartParsing();
        void StopParsing();
        void RestartParsing();
        Task DisposeOnAbort();
        void Dispose();
    }
}