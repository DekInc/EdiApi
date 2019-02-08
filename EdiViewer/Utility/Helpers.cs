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
                case 2:
                    return (new DateTime(Convert.ToInt32($"20{_Value.Substring(0, 2)}"),
                        Convert.ToInt32(_Value.Substring(2, 2)),
                        Convert.ToInt32(_Value.Substring(4, 2))
                        )).ToString(ApplicationSettings.DateTimeFormatTD);
            }
            return string.Empty;
        }
        public static string CodeToStr(this IHtmlHelper htmlHelper, string _Str, string _Param, IEnumerable<ComModels.LearCodes> _LearCodes, bool _Eng = true)
        {
            if (string.IsNullOrEmpty(_Param)) return string.Empty;
            IEnumerable<ComModels.LearCodes> Lc = _LearCodes.Where(C => C.Str == _Str && C.Param == _Param);
            if (Lc.Count() == 0)
                return $"{_Param} - Valor no mapeado";
            else
                return (_Eng ? Lc.FirstOrDefault().Value : Lc.FirstOrDefault().ValueEsp);
        }
        public static string QtyToMil(this IHtmlHelper htmlHelper, string _Str, IEnumerable<ComModels.LearUit830> _ListUits)
        {
            try
            {
                int Qty = Convert.ToInt32(_Str);
                if (_ListUits.Count() > 0)
                    return $"{Qty.ToString("N0")} {_ListUits.FirstOrDefault().UnitOfMeasure}";
                else
                    return Qty.ToString("N0");
            }
            catch
            {
                return _Str;
            }
        }
        public static string QtyToLocal(this IHtmlHelper htmlHelper, string _StrQty, IEnumerable<ComModels.LearShp830> _ShpQty, IEnumerable<ComModels.LearUit830> _ListUits)
        {
            try
            {
                if (_ShpQty.Where(S => S.QuantityQualifier.Equals("02")).Count() > 0)
                {
                    double ShpQty = Convert.ToDouble(_ShpQty.Where(S => S.QuantityQualifier.Equals("02")).FirstOrDefault().Quantity);
                    double Qty = Convert.ToDouble(_StrQty);
                    double QtyRes = (Qty - ShpQty);
                    if (_ListUits.Count() > 0)
                        return $"{QtyRes.ToString("N0")} {_ListUits.FirstOrDefault().UnitOfMeasure}";
                    else
                        return QtyRes.ToString("N0");
                }
                else return _StrQty;
            }
            catch
            {
                return _StrQty;
            }
        }
        public static object ShowLabel(this IHtmlHelper htmlHelper, string _ObjectLabel, IEnumerable<ComModels.LearCodes> _LearCodes)
        {
            var Div = new TagBuilder("span");
            Div.AddCssClass("dtColName");
            if (string.IsNullOrEmpty(_ObjectLabel)) return string.Empty;
            IEnumerable<ComModels.LearCodes> Lc = _LearCodes.Where(C => C.Str == _ObjectLabel);
            if (Lc.Count() == 0)
                Div.InnerHtml.AppendHtml($"[{_ObjectLabel}] - valor no mapeado<br/>");
            else
            {
                if (ApplicationSettings.English)
                {
                    if (string.IsNullOrEmpty(Lc.FirstOrDefault().Value))
                        Div.InnerHtml.AppendHtml($"[{_ObjectLabel}] - valor no mapeado<br/>");
                    else
                        Div.InnerHtml.AppendHtml($"{Lc.FirstOrDefault().Value}<br/>"); // English
                }
                else
                {
                    if (string.IsNullOrEmpty(Lc.FirstOrDefault().ValueEsp))
                        Div.InnerHtml.AppendHtml($"[{_ObjectLabel}] - valor no mapeado<br/>");
                    else
                        Div.InnerHtml.AppendHtml($"{Lc.FirstOrDefault().ValueEsp}<br/>"); // Spanish
                }
            }
            return Div;
        }
    }
}
