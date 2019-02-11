using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EdiApi.Models
{
    public static class ManualDB
    {
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSN(ref Models.WmsDB.WmsContext _WmsDbO)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _WmsDbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = "[dbo].[GetSN]";
                _WmsDbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListSn.Add(new TsqlDespachosWmsComplex()
                        {
                            DespachoId = Dr.GetInt32(0),
                            FechaSalida = Dr.GetDateTime(1),
                            CodProducto = Dr.GetString(2),
                            Producto = Dr.GetString(3),
                            Cliente = Dr.GetString(4),
                            Quantity = Dr.GetDouble(5),
                            Weight = Dr.GetDouble(6),
                            Volume = Dr.GetDouble(7),
                            Bulks = Dr.GetDouble(8),
                            UnidadDeMedida = Dr.GetString(9),
                            Destino = Dr.GetString(10),
                            Procesado = Dr.GetInt32(11)
                        });
                    }
                }
                _WmsDbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSNDet(ref Models.WmsDB.WmsContext _WmsDbO)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _WmsDbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[GetSNDet]";
                _WmsDbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListSn.Add(new TsqlDespachosWmsComplex()
                        {
                            DespachoId = Dr.GetInt32(0),
                            FechaSalida = Dr.GetDateTime(1),
                            CodProducto = Dr.GetString(2),
                            Producto = Dr.GetString(3),
                            Cliente = Dr.GetString(4),
                            Quantity = Dr.GetDouble(5),
                            Weight = Dr.GetDouble(6),
                            Volume = Dr.GetDouble(7),
                            Bulks = Dr.GetDouble(8),
                            UnidadDeMedida = Dr.GetString(9),
                            Destino = Dr.GetString(10),
                            
                            NoContenedor = Dr.GetString(11),
                            Motorista = Dr.GetString(12),
                            DocumentoMotorista = Dr.GetString(13),
                            DocumentoFiscal = Dr.GetString(14),
                            FechaDocFiscal = Dr.GetDateTime(15),
                            NoMarchamo = Dr.GetString(16),
                            Observacion = Dr.GetString(17), 
                            TotalValue = Dr.GetDouble(18),
                            NumeroOc = Dr.GetString(19)
                        });
                    }
                }
                _WmsDbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<FE830DataAux> SP_GetExistencias(ref Models.WmsDB.WmsContext _WmsDbO, int _IdClient)
        {
            List<FE830DataAux> ListExists = new List<FE830DataAux>();
            using (DbCommand Cmd = _WmsDbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetExistencias] {_IdClient}";
                _WmsDbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new FE830DataAux()
                        {                            
                            CodProducto = Dr.GetString(0),
                            Producto = Dr.GetString(1),
                            Existencia = Dr.GetDouble(2),
                            UnidadDeMedida = Dr.GetString(3)
                        });
                    }
                }
                _WmsDbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
    }
}
