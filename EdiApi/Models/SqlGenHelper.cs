using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public static class SqlGenHelper {
        public static string GetSqlWmsMaxTbl(string TableName, string Pk) {
            return $"SELECT @MaxTransaccionId = ISNULL(MAX({Pk}), 0) + 1 FROM wms.dbo.{TableName};" + Environment.NewLine;
        }
        public static string GetSqlWmsInsertTransacciones() {
            return $"SELECT @MaxTransaccionId = ISNULL(MAX({Pk}), 0) + 1 FROM wms.dbo.{TableName};" + Environment.NewLine;
        }
    }
}
