using BLL.InterfaceAccessors;
using BLL.Models;
using DisksParserUI.Stores;

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