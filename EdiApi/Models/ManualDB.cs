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
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSN(ref Models.EdiDB.EdiDBContext _DbO)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = "[dbo].[GetSN]";
                _DbO.Database.OpenConnection();
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
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSNDet(ref Models.EdiDB.EdiDBContext _DbO)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[GetSNDet]";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListSn.Add(new TsqlDespachosWmsComplex()
                        {
                            DespachoId = Convert.ToInt32(Dr.GetValue(0)),
                            FechaSalida = Convert.ToDateTime(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            Producto = Convert.ToString(Dr.GetValue(3)),
                            Cliente = Convert.ToString(Dr.GetValue(4)),
                            Quantity = Convert.ToDouble(Dr.GetValue(5)),
                            Weight = Convert.ToDouble(Dr.GetValue(6)),
                            Volume = Convert.ToDouble(Dr.GetValue(7)),
                            Bulks = Convert.ToDouble(Dr.GetValue(8)),
                            UnidadDeMedida = Convert.ToString(Dr.GetValue(9)),
                            Destino = Convert.ToString(Dr.GetValue(10)),

                            NoContenedor = Convert.ToString(Dr.GetValue(11)),
                            Motorista = Convert.ToString(Dr.GetValue(12)),
                            DocumentoMotorista = Convert.ToString(Dr.GetValue(13)),
                            DocumentoFiscal = Convert.ToString(Dr.GetValue(14)),
                            FechaDocFiscal = Convert.ToDateTime(Dr.GetValue(15)),
                            NoMarchamo = Convert.ToString(Dr.GetValue(16)),
                            Observacion = Convert.ToString(Dr.GetValue(17)), 
                            TotalValue = Convert.ToDouble(Dr.GetValue(20)),
                            NumeroOc = Convert.ToString(Dr.GetValue(21))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<FE830DataAux> SP_GetExistencias(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<FE830DataAux> ListExists = new List<FE830DataAux>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetExistencias] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new FE830DataAux()
                        {                            
                            CodProducto = Convert.ToString(Dr.GetValue(0)),
                            Producto = Convert.ToString(Dr.GetValue(1)),
                            Existencia = Convert.ToDouble(Dr.GetValue(2)),
                            UnidadDeMedida = Convert.ToString(Dr.GetValue(3)),
                            CodProductoLear = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<ExistenciasExternModel> SP_GetExistenciasExtern(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<ExistenciasExternModel> ListExists = new List<ExistenciasExternModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetExistenciasExtern] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new ExistenciasExternModel()
                        {
                            Cliente = Convert.ToString(Dr.GetValue(0)),
                            Bodega = Convert.ToString(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            Descripcion = Convert.ToString(Dr.GetValue(3)),
                            Existencia = Convert.ToDouble(Dr.GetValue(4)),
                            Reservado = Convert.ToDouble(Dr.GetValue(5)),
                            Disponible = Convert.ToDouble(Dr.GetValue(6)),
                            ClienteID = Convert.ToInt32(Dr.GetValue(7)),
                            BodegaID = Convert.ToInt32(Dr.GetValue(8)),
                            Bultos = Convert.ToInt32(Dr.GetValue(9)),
                            Peso = Convert.ToDouble(Dr.GetValue(10)),
                            Volumen = Convert.ToDouble(Dr.GetValue(11)),
                            Uxb = Convert.ToInt32(Dr.GetValue(12)),
                            Lote = Convert.ToString(Dr.GetValue(13)),
                            Contenedor = Convert.ToString(Dr.GetValue(14))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
    }
}
