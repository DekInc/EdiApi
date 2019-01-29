using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Utility.Helper
{
    public static class Helpers
    {
        public static string StrToDate(this IHtmlHelper htmlHelper, int _Option, string _Value, string _ValueT = "")
        {
            if (string.IsNullOrEmpty(_Value)) return string.Empty;
            switch (_Option) {
                case 0:
                    return (new DateTime(Convert.ToInt32($"20{_Value.Substring(0, 2)}"), 
                        Convert.ToInt32(_Value.Substring(2, 2)),
                        Convert.ToInt32(_Value.Substring(4, 2)),
                        Convert.ToInt32(_ValueT.Substring(0, 2)),
                        Convert.ToInt32(_ValueT.Substring(2, 2)), 0
                        )).ToString(ApplicationSettings.DateTimeFormatT);
                case 1:
                    return (new DateTime(Convert.ToInt32($"20{_Value.Substring(0, 2)}"),
                        Convert.ToInt32(_Value.Substring(2, 2)),
                        Convert.ToInt32(_Value.Substring(4, 2))
                        )).ToString(ApplicationSettings.DateTimeFormat);
            }
            return string.Empty;
        }
    }
}
