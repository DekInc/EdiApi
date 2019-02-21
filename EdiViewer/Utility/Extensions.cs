using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer
{
    public static class Extensions
    {
        public static void SetObjSession(this ISession Session, string _Key, object _Val)
        {
            Session.SetString(_Key, JsonConvert.SerializeObject(_Val));
        }
        public static T GetObjSession<T>(this ISession Session, string _Key)
        {
            string Val = Session.GetString(_Key);
            return Val == null ? default(T) : JsonConvert.DeserializeObject<T>(Val);
        }
        public static bool ExistsObjSession<T>(this ISession Session, string _Key)
        {
            string Val = Session.GetString(_Key);
            return Val == null ? false : true;
        }
        public static TSource Fod<TSource>(this IEnumerable<TSource> source) {
            return source.FirstOrDefault();
        }
        public static DateTime ToDate(this string _Str)
        {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"{_Str.Substring(6, 4)}"),
                        Convert.ToInt32(_Str.Substring(3, 2)),
                        Convert.ToInt32(_Str.Substring(0, 2)),
                        Convert.ToInt32(_Str.Substring(11, 2)),
                        Convert.ToInt32(_Str.Substring(14, 2)), 0
                        );
        }
        public static DateTime ToDateEsp(this string _Str)
        {
            if (string.IsNullOrEmpty(_Str)) return DateTime.Now;

            return new DateTime(Convert.ToInt32($"{_Str.Substring(4, 4)}"),
                        Convert.ToInt32(_Str.Substring(2, 2)),
                        Convert.ToInt32(_Str.Substring(0, 2))
                        );
        }
    }
}
