using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public static class Extensions
    {
        public static void AddParentChild(this EdiBase _EdiBase, EdiBase _NewChild)
        {
            _NewChild.Parent = _EdiBase;
            _EdiBase.Childs.Add(_NewChild);
        }
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
        public static TSource Fod<TSource>(this IEnumerable<TSource> source)
        {
            return source.FirstOrDefault();
        }
        public static bool IsNumeric(this char CharO)
        {
            bool isNum = Double.TryParse(Convert.ToString(CharO), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double retNum);
            return isNum;
        }
        public static string GiveFirstStr(this string _Code)
        {
            try
            {
                int I = 0;
                for (I = 0; I < _Code.Length; I++)
                    if (_Code[I].IsNumeric())
                        break;
                return _Code.Substring(0, I);
            }
            catch
            {
                return _Code;
            }            
        }
        public static DateTime ToShortDate(this string _Str)
        {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"20{_Str.Substring(0, 2)}"),
                        Convert.ToInt32(_Str.Substring(2, 2)),
                        Convert.ToInt32(_Str.Substring(4, 2))
                        );
        }
        public static DateTime ToDate(this string _Str)
        {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"20{_Str.Substring(0, 2)}"),
                        Convert.ToInt32(_Str.Substring(2, 2)),
                        Convert.ToInt32(_Str.Substring(4, 2)),
                        Convert.ToInt32(_Str.Substring(6, 2)),
                        Convert.ToInt32(_Str.Substring(8, 2)), 0
                        );
        }
        public static DateTime ToDateEsp(this string _Str)
        {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"{_Str.Substring(6, 4)}"),
                        Convert.ToInt32(_Str.Substring(3, 2)),
                        Convert.ToInt32(_Str.Substring(0, 2)),
                        Convert.ToInt32(_Str.Substring(11, 2)),
                        Convert.ToInt32(_Str.Substring(14, 2)), 0
                        );
        }
        public static DateTime ToDateFromEspDate(this string _Str) {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"{_Str.Substring(6, 4)}"),
                        Convert.ToInt32(_Str.Substring(3, 2)),
                        Convert.ToInt32(_Str.Substring(0, 2))
                        );
        }
        public static string ToSqlDate(this DateTime _D) {
            return $"CONVERT(DATETIME, '{_D.ToString(ApplicationSettings.DateTimeFormatShort)}', 103)";
        }
        public static string ToSqlDate(this DateTime? _D) {
            return $"CONVERT(DATETIME, '{_D.Value.ToString(ApplicationSettings.DateTimeFormatShort)}', 103)";
        }
        public static T Gr<T>(this System.Data.Common.DbDataReader Dr, int I) {
            if (Dr.IsDBNull(I)) return default(T);
            return (T)Dr.GetValue(I);
        }
    }
}
