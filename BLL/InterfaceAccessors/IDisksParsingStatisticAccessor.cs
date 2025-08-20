using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InterfaceAccessors
{
    public interface IDisksParsingStatisticAccessor
    {
        DisksParsingStatistic GetDisksParsingStatistic();
    }
}