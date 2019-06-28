using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ApplicationSettings
    {
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public const string DateTimeFormatSqlServer = "dd-MM-yyyy";
        public const string DateTimeFormatSqlServerInsert = "yyyy-MM-dd HH:mm";
        public const string DateTimeFormatShort = "dd/MM/yyyy";
        public const string ToDateTimeFormat = "yyMMdd";
        public const string ToTimeFormat = "HHmm";
        public const string ToTimeFormatExcel = "HH:mm";
        public const string DateTimeFormatEdiFrom = "yyMMddHHmm";
        public static int SavedSegments = 0;
    }    
}
