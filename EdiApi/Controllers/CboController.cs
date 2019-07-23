using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models;
using EdiApi.Models.EdiDB;
using EdiApi.Models.WmsDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CboController : ControllerBase {
        public EdiDBContext DbO;
        public EdiDBContext DbOLong;
        public WmsContext WmsDbO;
        public WmsContext WmsDbOLong;
        public static readonly string G1;
        public readonly IConfiguration Config;
        IConfiguration IMapConfig => Config.GetSection("IMapConfig");
        IConfiguration IEdiFtpConfig => Config.GetSection("EdiFtp");
        string IMapHost => (string)IMapConfig.GetValue(typeof(string), "Host");
        int IMapPortIn => Convert.ToInt32(IMapConfig.GetValue(typeof(string), "PortIn"));
        int IMapPortOut => Convert.ToInt32(IMapConfig.GetValue(typeof(string), "PortOut"));
        bool IMapSSL => Convert.ToBoolean(IMapConfig.GetValue(typeof(string), "SSL"));
        string IMapUser => (string)IMapConfig.GetValue(typeof(string), "User");
        string IMapPassword => (string)IMapConfig.GetValue(typeof(string), "Password");
        string FtpHost => (string)IEdiFtpConfig.GetValue(typeof(string), "Host");
        string FtpHostFailover => (string)IEdiFtpConfig.GetValue(typeof(string), "HostFailover");
        string FtpUser => (string)IEdiFtpConfig.GetValue(typeof(string), "EdiUser");
        string FtpPassword => (string)IEdiFtpConfig.GetValue(typeof(string), "EdiPassword");
        string FtpDirIn => (string)IEdiFtpConfig.GetValue(typeof(string), "DirIn");
        string FtpDirOut => (string)IEdiFtpConfig.GetValue(typeof(string), "DirOut");
        string FtpDirChecked => (string)IEdiFtpConfig.GetValue(typeof(string), "DirChecked");
        object MaxEdiComs => Config.GetSection("MaxEdiComs").GetValue(typeof(object), "Value");
        public CboController(EdiDBContext _DbO, EdiDBContext _DbOL, WmsContext _WmsDbO, WmsContext _WmsDbOLong, IConfiguration _Config) {
            DbO = _DbO;
            DbOLong = _DbOL;
            WmsDbO = _WmsDbO;
            WmsDbOLong = _WmsDbOLong;
            Config = _Config;
            DbO.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
            DbOLong.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
            WmsDbO.Database.SetCommandTimeout(TimeSpan.FromMinutes(4));
            WmsDbOLong.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
        }
        private IEnumerable<Rep830Info> GetExToIe1(Exception E1) {
            yield return new Rep830Info() { errorMessage = E1.ToString() };
        }
        private IEnumerable<TsqlDespachosWmsComplex> GetExToIe2(Exception E1) {
            yield return new TsqlDespachosWmsComplex() { ErrorMessage = E1.ToString() };
        }
        [HttpGet]
        public RetData<IEnumerable<CboValuesModel>> GetPaylessEncuestaCboPedidos(int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                int DaysToAdd = 0;
                switch (StartTime.DayOfWeek) {                    
                    case DayOfWeek.Monday:
                        DaysToAdd = -5;
                        break;
                    case DayOfWeek.Tuesday:
                        DaysToAdd = -6;
                        break;
                    case DayOfWeek.Wednesday:
                        DaysToAdd = -7;
                        break;
                    case DayOfWeek.Thursday:
                        DaysToAdd = -8;
                        break;
                    case DayOfWeek.Friday:
                        DaysToAdd = -9;
                        break;
                    case DayOfWeek.Saturday:
                        DaysToAdd = -10;
                        break;
                    case DayOfWeek.Sunday:
                        DaysToAdd = -11;
                        break;                    
                    default:
                        break;
                }
                List<CboValuesModel> ListOrders = (
                    from Pe in DbO.PedidosExternos
                    //from E in DbO.PaylessEncuestaResM
                    where Pe.TiendaId == TiendaId
                    && Pe.PedidoWms != null
                    //&& Pe.Id != Convert.ToInt32(E.Pedido)
                    && Pe.FechaPedido.ToDateEsp() >= StartTime.AddDays(DaysToAdd)
                    && Pe.FechaPedido.ToDateEsp() <= StartTime.AddDays(DaysToAdd + 7)
                    orderby Pe.Id
                    select new CboValuesModel {
                        V = Pe.Id.ToString(),
                        T = $"# {Pe.Id} - {Pe.FechaPedido} "
                    }
                    ).Distinct().ToList();
                //foreach (PaylessEncuestaResM ResM in DbO.PaylessEncuestaResM) {
                //    if (ListOrders.Where(O => O.V == ResM.Pedido.ToString()).Count() > 0)
                //        ListOrders.RemoveAll(O => O.V == ResM.Pedido.ToString());
                //}
                return new RetData<IEnumerable<CboValuesModel>> {
                    Data = ListOrders,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<CboValuesModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessProdPrioriDet>> GetFilterTemporada() {
            DateTime StartTime = DateTime.Now;
            try {                
                return new RetData<IEnumerable<PaylessProdPrioriDet>> {
                    Data = DbO.PaylessProdPrioriDet.Take(3200),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessProdPrioriDet>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
    }
}