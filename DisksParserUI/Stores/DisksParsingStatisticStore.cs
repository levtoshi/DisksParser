using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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