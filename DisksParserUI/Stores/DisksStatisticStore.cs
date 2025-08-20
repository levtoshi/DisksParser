using BLL.Models;

namespace DisksParserUI.Stores
{
    public class DisksStatisticStore
    {
        private readonly DisksStatistic _disksStatisticObject;
        public DisksStatistic DisksStatisticObject
        {
            get
            {
                return _disksStatisticObject;
            }
        }

        public DisksStatisticStore()
        {
            _disksStatisticObject = new DisksStatistic();
        }
    }
}