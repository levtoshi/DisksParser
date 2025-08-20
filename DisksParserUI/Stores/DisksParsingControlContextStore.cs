using BLL.Models;

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