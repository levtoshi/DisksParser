using BLL.Models;

namespace DisksParserUI.Stores
{
    public class DisksParsingStatisticStore
    {
        private readonly DisksParsingStatistic _disksParsingStatisticObject;
        public DisksParsingStatistic DisksParsingStatisticObject
        {
            get
            {
                return _disksParsingStatisticObject;
            }
        }

        public DisksParsingStatisticStore()
        {
            _disksParsingStatisticObject = new DisksParsingStatistic();
        }
    }
}