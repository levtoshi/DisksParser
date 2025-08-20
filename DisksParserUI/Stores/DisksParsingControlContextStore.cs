using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisksParserUI.Stores
{
    public class DisksParsingControlContextStore
    {
        private readonly DisksParsingControlContext _disksParsingControlContextObject;
        public DisksParsingControlContext DisksParsingControlContextObject
        {
            get
            {
                return _disksParsingControlContextObject;
            }
        }

        public DisksParsingControlContextStore()
        {
            _disksParsingControlContextObject = new DisksParsingControlContext();
        }
    }
}