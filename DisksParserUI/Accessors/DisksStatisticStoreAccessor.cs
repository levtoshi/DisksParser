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
    public class DisksStatisticStoreAccessor : IDisksStatisticAccessor
    {
        private readonly DisksStatisticStore _disksStatisticStore;

        public DisksStatisticStoreAccessor(DisksStatisticStore disksStatisticStore)
        {
            _disksStatisticStore = disksStatisticStore;
        }

        public DisksStatistic GetDisksStatistic()
        {
            return _disksStatisticStore.DisksStatisticObject;
        }
    }
}