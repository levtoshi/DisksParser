using BLL.InterfaceAccessors;
using BLL.Models;
using DisksParserUI.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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