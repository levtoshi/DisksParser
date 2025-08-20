using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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