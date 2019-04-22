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

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public EdiDBContext DbO;
        public WmsContext WmsDbO;
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
        public AccountController(EdiDBContext _DbO, WmsContext _WmsDbO, IConfiguration _Config)
        {
            DbO = _DbO;
            WmsDbO = _WmsDbO;
            Config = _Config;
        }
        [HttpPost]
        public RetData<IEnumerable<IenetUsers>> GetUsers(object HashId)
        {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try
            {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser))
                {
                    List<IenetUsers> ListUsers = DbO.IenetUsers.ToList();
                    ListUsers.ForEach(O => O.UsrPassword = string.Empty);
                    return new RetData<IEnumerable<IenetUsers>>
                    {
                        Data = ListUsers,
                        Info = new RetInfo()
                        {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                else {
                    return new RetData<IEnumerable<IenetUsers>>
                    {
                        Info = new RetInfo()
                        {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<IenetUsers>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<IenetGroups>> GetGroups(object HashId)
        {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try
            {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser))
                {
                    List<IenetGroups> ListUsers = DbO.IenetGroups.ToList();
                    return new RetData<IEnumerable<IenetGroups>>
                    {
                        Data = ListUsers,
                        Info = new RetInfo()
                        {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                else
                {
                    return new RetData<IEnumerable<IenetGroups>>
                    {
                        Info = new RetInfo()
                        {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<IenetGroups>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<IenetAccesses>> GetIenetAccesses(object HashId)
        {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try
            {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser))
                {
                    List<IenetAccesses> ListUsers = DbO.IenetAccesses.ToList();
                    return new RetData<IEnumerable<IenetAccesses>>
                    {
                        Data = ListUsers,
                        Info = new RetInfo()
                        {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                else
                {
                    return new RetData<IEnumerable<IenetAccesses>>
                    {
                        Info = new RetInfo()
                        {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<IenetAccesses>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<IenetGroupsAccesses>> GetIEnetGroupsAccesses(object HashId)
        {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try
            {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser))
                {
                    List<IenetGroupsAccesses> ListUsers = DbO.IenetGroupsAccesses.ToList();
                    return new RetData<IEnumerable<IenetGroupsAccesses>>
                    {
                        Data = ListUsers,
                        Info = new RetInfo()
                        {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                else
                {
                    return new RetData<IEnumerable<IenetGroupsAccesses>>
                    {
                        Info = new RetInfo()
                        {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<IenetGroupsAccesses>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }        
        [HttpPost]
        public string LoginIe(UserModel UserEnc)
        {
            try
            {
                string UserDecrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(UserEnc.User)));
                string PasswordDecrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(UserEnc.Password)));
                IenetUsers UserO = (
                    from U in DbO.IenetUsers
                    where U.CodUsr == UserDecrypted
                    && U.UsrPassword == PasswordDecrypted
                    select U
                    ).Fod();
                if (UserO == null)
                    return string.Empty;
                UserO.HashId = EdiBase.GetHashId();
                DbO.IenetUsers.Update(UserO);
                DbO.SaveChanges();
                string Ret = $"UserO.IdIenetGroup|{UserO.IdIenetGroup}|UserO.ClientId|{UserO.ClienteId}|UserO.HashId|{UserO.HashId}";
                Ret = Convert.ToBase64String(CryptoHelper.EncryptData(Encoding.UTF8.GetBytes(Ret)));
                return Ret;
            }
            catch (Exception e1)
            {
                return "Error: " + e1.ToString();
            }
        }
    }
}