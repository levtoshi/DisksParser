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
    public class DisksParsingStatisticStoreAccessor : IDisksParsingStatisticAccessor
    {
        private readonly DisksParsingStatisticStore _disksParsingStatisticStore;
        public DisksParsingStatisticStoreAccessor(DisksParsingStatisticStore disksParsingStore)
        {
            _disksParsingStatisticStore = disksParsingStore;
        }

        public DisksParsingStatistic GetDisksParsingStatistic()
        {
            return _disksParsingStatisticStore.DisksParsingStatisticObject;
        }
    }
}