using ComModels;
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
        public static bool StrToBool(this IHtmlHelper htmlHelper, int _Option, string _Value, int ContI)
        {
            if (ContI > 6) return false;
            if (string.IsNullOrEmpty(_Value)) return false;
            DateTime ThisTime = (new DateTime(Convert.ToInt32($"20{_Value.Substring(0, 2)}"),
                        Convert.ToInt32(_Value.Substring(2, 2)),
                        Convert.ToInt32(_Value.Substring(4, 2))
                        ));
            return ((ThisTime.DayOfWeek == DayOfWeek.Monday)
                || (ThisTime.DayOfWeek == DayOfWeek.Wednesday)
                || (ThisTime.DayOfWeek == DayOfWeek.Friday));
        }
        public static string CodeToStr(this IHtmlHelper htmlHelper, string _Str, string _Param, IEnumerable<ComModels.LearCodes> _LearCodes, bool _Eng = true)
        {
            if (string.IsNullOrEmpty(_Param)) return string.Empty;
            IEnumerable<ComModels.LearCodes> Lc = _LearCodes.Where(C => C.Str == _Str && C.Param == _Param);
            if (Lc.Count() == 0)
                return $"{_Param} - Valor no mapeado";
            else
                return (_Eng ? Lc.Fod().Value : Lc.Fod().ValueEsp);
        }
        public static string QtyToMil(this IHtmlHelper htmlHelper, string _Str, IEnumerable<ComModels.LearUit830> _ListUits)
        {
            try
            {
                int Qty = Convert.ToInt32(_Str);
                if (_ListUits.Count() > 0)
                    return $"{Qty.ToString("N0")} {_ListUits.Fod().UnitOfMeasure}";
                else
                    return Qty.ToString("N0");
            }
            catch
            {
                return _Str;
            }
        }
        public static string QtyToMil(this IHtmlHelper htmlHelper, double _Str, string _Um)
        {
            try
            {                
                return $"{_Str.ToString("N0")} {_Um}";
            }
            catch
            {
                return _Str.ToString("N0");
            }
        }
        public static string QtyToLocal(this IHtmlHelper htmlHelper, string _StrQty, IEnumerable<ComModels.LearShp830> _ShpQty, IEnumerable<ComModels.LearUit830> _ListUits, string _Date, IEnumerable<EdiViewer.Models.EdiDetailQtysModel> _ListFstQtys, string _ParentHashId)
        {
            try
            {
                if (_ShpQty.Where(S => S.QuantityQualifier.Equals("02")).Count() > 0)
                {
                    DateTime ThisTime = (new DateTime(Convert.ToInt32($"20{_Date.Substring(0, 2)}"),
                        Convert.ToInt32(_Date.Substring(2, 2)),
                        Convert.ToInt32(_Date.Substring(4, 2))
                        ));                    
                    if (_ListUits.Count() > 0)
                    {
                        switch (ThisTime.DayOfWeek) {
                            case DayOfWeek.Monday:
                                double ShpQty = Convert.ToDouble(_ShpQty.Where(S => S.QuantityQualifier.Equals("02")).Fod().Quantity);
                                double Qty = Convert.ToDouble(_StrQty);
                                double QtyRes = (Qty - ShpQty);
                                return $"{QtyRes.ToString("N0")} {_ListUits.Fod().UnitOfMeasure}";
                            case DayOfWeek.Wednesday:
                                Models.EdiDetailQtysModel QtyLast = _ListFstQtys.Where(FQ => FQ.FstDate == ThisTime.AddDays(-2) && FQ.HashId == _ParentHashId).Fod();
                                Models.EdiDetailQtysModel QtyNext = _ListFstQtys.Where(FQ => FQ.FstDate == ThisTime && FQ.HashId == _ParentHashId).Fod();
                                if (QtyNext != null && QtyLast != null)
                                {
                                    return $"{(QtyNext.Qty - QtyLast.Qty).ToString("N0")} {_ListUits.Fod().UnitOfMeasure}";
                                }
                                else return "-1";
                            case DayOfWeek.Friday:
                                Models.EdiDetailQtysModel QtyLast2 = _ListFstQtys.Where(FQ => FQ.FstDate == ThisTime.AddDays(-2) && FQ.HashId == _ParentHashId).Fod();
                                Models.EdiDetailQtysModel QtyNext2 = _ListFstQtys.Where(FQ => FQ.FstDate == ThisTime && FQ.HashId == _ParentHashId).Fod();
                                if (QtyNext2 != null && QtyLast2 != null)
                                {
                                    return $"{(QtyNext2.Qty - QtyLast2.Qty).ToString("N0")} {_ListUits.Fod().UnitOfMeasure}";
                                }
                                else return "-1";
                        }                        
                        return "-1";
                    }
                    else
                        return _StrQty;
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
                    if (string.IsNullOrEmpty(Lc.Fod().Value))
                        Div.InnerHtml.AppendHtml($"[{_ObjectLabel}] - valor no mapeado<br/>");
                    else
                        Div.InnerHtml.AppendHtml($"{Lc.Fod().Value}<br/>"); // English
                }
                else
                {
                    if (string.IsNullOrEmpty(Lc.Fod().ValueEsp))
                        Div.InnerHtml.AppendHtml($"[{_ObjectLabel}] - valor no mapeado<br/>");
                    else
                        Div.InnerHtml.AppendHtml($"{Lc.Fod().ValueEsp}<br/>"); // Spanish
                }
            }
            return Div;
        }
        public static string ShowVersion(this IHtmlHelper htmlHelper)
        {
            return "versión 1.1.1.0";
        }        
    }
}
