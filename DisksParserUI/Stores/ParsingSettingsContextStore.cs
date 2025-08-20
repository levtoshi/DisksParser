using BLL.Models;

namespace DisksParserUI.Stores
{
    public class ParsingSettingsContextStore
    {
        private readonly ParsingSettingsContext _parsingSettingsContextObject;
        public ParsingSettingsContext ParsingSettingsContextObject
        {
            get
            {
                return _parsingSettingsContextObject;
            }
        }

        public ParsingSettingsContextStore()
        {
            _parsingSettingsContextObject = new ParsingSettingsContext();
        }
    }
}