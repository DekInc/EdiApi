using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public static class Extensions
    {
        public static void AddLastChild(this List<LIN830> _ListLIN, EdiBase _NewChild)
        {
            _ListLIN.LastOrDefault().Childs.Add(_NewChild);
        }
        public static void AddLastChild(this List<SDP830> _ListSDP, EdiBase _NewChild)
        {
            _ListSDP.LastOrDefault().Childs.Add(_NewChild);
        }
        public static void AddLastChild(this List<SHP830> _ListSHP, EdiBase _NewChild)
        {
            _ListSHP.LastOrDefault().Childs.Add(_NewChild);
        }
    }
}
