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
        public static TSource Fod<TSource>(this IEnumerable<TSource> source) {
            return source.FirstOrDefault();
        }
    }
}
