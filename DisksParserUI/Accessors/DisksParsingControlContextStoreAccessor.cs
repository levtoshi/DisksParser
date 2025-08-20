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