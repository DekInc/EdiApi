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
    }
}
