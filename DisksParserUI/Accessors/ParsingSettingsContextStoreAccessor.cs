using BLL.InterfaceAccessors;
using BLL.Models;
using DisksParserUI.Stores;

namespace DisksParserUI.Accessors
{
    public class ParsingSettingsContextStoreAccessor : IParsingSettingsContextAccessor
    {
        private readonly ParsingSettingsContextStore _parsingSettingsContextStore;

        public ParsingSettingsContextStoreAccessor(ParsingSettingsContextStore parsingSettingsContextStore)
        {
            _parsingSettingsContextStore = parsingSettingsContextStore;
        }

        public ParsingSettingsContext GetParsingSettingsContext()
        {
            return _parsingSettingsContextStore.ParsingSettingsContextObject;
        }
    }
}