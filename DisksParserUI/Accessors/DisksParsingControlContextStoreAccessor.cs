using BLL.InterfaceAccessors;
using BLL.Models;
using DisksParserUI.Stores;

namespace DisksParserUI.Accessors
{
    public class DisksParsingControlContextStoreAccessor : IDisksParsingControlContextAccessor
    {
        private readonly DisksParsingControlContextStore _disksParsingControlContextStore;
        public DisksParsingControlContextStoreAccessor(DisksParsingControlContextStore disksParsingControlContextStore)
        {
            _disksParsingControlContextStore = disksParsingControlContextStore;
        }

        public DisksParsingControlContext GetDisksParsingControlContext()
        {
            return _disksParsingControlContextStore.DisksParsingControlContextObject;
        }
    }
}