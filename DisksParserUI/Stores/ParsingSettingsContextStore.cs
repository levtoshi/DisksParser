using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisksParserUI.Stores
{
    public class ParsingSettingsContextStore
    {
        private readonly ParsingSettingsContext _parsingSettingsContextObject;
        public ParsingSettingsContext ParsingSettingsContextObject
        {
            get
            {
                return _parsingSettingsContextObject;
            }
        }

        public ParsingSettingsContextStore()
        {
            _parsingSettingsContextObject = new ParsingSettingsContext();
        }
    }
}