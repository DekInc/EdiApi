using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreApiClient;
using System.Diagnostics;
using System.Text;
using System.Net;
using ComModels;
using EdiViewer.Models;

namespace EdiViewer.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View(new Models.ErrorModel());
        }
        public IActionResult CrudUsuarios()
        {
            return View();
        }
        public IActionResult CrudGrupos()
        {
            return View();
        }
        public IActionResult CrudAccesos()
        {
            return View();
        }
        [HttpGet]
        public bool MiAlive()
        {
            string CodUsr = HttpContext.Session.GetObjSession<string>("Session.CodUsr");
            if (string.IsNullOrEmpty(CodUsr)) return false;
            return true;
        }
        public async Task<IEnumerable<IenetUsersModel>> GetUsers()
        {
            try
            {
                RetData<IEnumerable<IenetUsers>> ListUsers = await ApiClientFactory.Instance.GetUsers(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                //if (ListUsers.Info.CodError != 0) {
                //    return new RetData<IEnumerable<IenetUsersModel>>
                //    {
                //        Info = ListUsers.Info
                //    };
                //}
                IEnumerable<IenetGroups> ListGroups = HttpContext.Session.GetObjSession<IEnumerable<IenetGroups>>("ListGroups");
                RetData<IEnumerable<Clientes>> ListClients = await ApiClientFactory.Instance.GetAllClients(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                //if (ListClients.Info.CodError != 0)
                //{
                //    return new RetData<IEnumerable<IenetUsersModel>>
                //    {
                //        Info = ListUsers.Info
                //    };
                //}
                List<IenetUsersModel> ListUserM = new List<IenetUsersModel>();
                foreach (IenetUsers UserO in ListUsers.Data)
                {
                    ListUserM.Add(new IenetUsersModel() {
                        Id = UserO.Id,
                        CodUsr = UserO.CodUsr,
                        IdIenetGroup = UserO.IdIenetGroup,
                        NomUsr = UserO.NomUsr,
                        ClienteId = UserO.ClienteId,
                        Cliente = (UserO.ClienteId.HasValue? ListClients.Data.Where(C => C.ClienteId == UserO.ClienteId).Fod().Nombre : ""),
                        IenetGroup = ListGroups.Where(G => G.Id == UserO.IdIenetGroup).Fod().Descr
                    });
                }
                return ListUserM;
                //return new RetData<IEnumerable<IenetUsersModel>>
                //{
                //    Data = ListUserM,
                //    Info = new RetInfo()
                //    {
                //        CodError = 0,
                //        Mensaje = "ok",
                //        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                //    }
                //};
            }
            catch // (Exception e1)
            {
                return new List<IenetUsersModel>();
                //return new RetData<IEnumerable<IenetUsersModel>>
                //{
                //    Info = new RetInfo()
                //    {
                //        CodError = -1,
                //        Mensaje = e1.ToString(),
                //        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                //    }
                //};
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(string TxtUser, string TxtPassword) {
            try
            {
                string HashId = await ApiClientFactory.Instance.Login(TxtUser, TxtPassword);
                bool IsExtern = false;
                if (HashId.StartsWith("Error:"))
                    return View("Index", new Models.ErrorModel() { ErrorMessage = System.Net.WebUtility.HtmlEncode(HashId.Replace(Environment.NewLine, "<br />")) });
                if (string.IsNullOrEmpty(HashId))
                {
                    string UserEncrypted = ApiClientFactory.Instance.Encrypt(TxtUser);
                    string PasswordEncrypted = ApiClientFactory.Instance.Encrypt(TxtPassword);
                    HashId = await ApiClientFactory.Instance.LoginExtern(UserEncrypted, PasswordEncrypted);
                    if (string.IsNullOrEmpty(HashId))
                        return LocalRedirect("/Account/?error=USER_INCORRECT");
                    HttpContext.Session.SetObjSession("Session.IsExtern", true);
                    IsExtern = true;
                    HttpContext.Session.SetObjSession("Session.ClientId", HashId.Split('|')[1]);
                    HashId = HashId.Split('|')[0];
                } else
                {
                    HttpContext.Session.SetObjSession("Session.IsExtern", false);
                    HashId += DateTime.Now.ToString(ApplicationSettings.DateTimeFormatL);
                }
                if (HashId.StartsWith("Error:"))
                    return View("Index", new Models.ErrorModel() { ErrorMessage = System.Net.WebUtility.HtmlEncode(HashId.Replace(Environment.NewLine, "<br />")) });
                HttpContext.Session.SetObjSession("Session.HashId", HashId);
                HttpContext.Session.SetObjSession("Session.CodUsr", TxtUser);
                if (string.IsNullOrEmpty(HashId))
                    return LocalRedirect("/Account/?error=USER_INCORRECT");
                if (IsExtern)
                    return LocalRedirect("/HomeExtern/Inventario");
                else
                    return LocalRedirect("/");
            }
            catch (Exception e1)
            {
                return View("Index", new Models.ErrorModel() { ErrorMessage = e1.ToString().Replace("'", "") });
            }            
        }
        [HttpPost]
        public async Task<IActionResult> LoginIe(string TxtUser, string TxtPassword)
        {
            try
            {
                string UserEncrypted = ApiClientFactory.Instance.Encrypt(TxtUser);
                string PasswordEncrypted = ApiClientFactory.Instance.Encrypt(TxtPassword);
                string HashId = await ApiClientFactory.Instance.LoginIe(UserEncrypted, PasswordEncrypted);
                if (string.IsNullOrEmpty(HashId))
                    return LocalRedirect("/Account/?error=USER_INCORRECT");                
                if (HashId.StartsWith("Error:"))
                    return View("Index", new Models.ErrorModel() { ErrorMessage = System.Net.WebUtility.HtmlEncode(HashId.Replace(Environment.NewLine, "<br />")) });
                if (string.IsNullOrEmpty(HashId))
                    return LocalRedirect("/Account/?error=USER_INCORRECT");
                string HashIdDecrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(HashId)));
                if (HashIdDecrypted.Split("|").Length != 6)
                    return View("Index", new Models.ErrorModel() { ErrorMessage = "Error en el sistema de auth." });
                HttpContext.Session.SetObjSession("Session.ClientId", HashIdDecrypted.Split("|")[3]);
                HttpContext.Session.SetObjSession("Session.HashId", HashIdDecrypted.Split("|")[5]);
                HttpContext.Session.SetObjSession("Session.IdGroup", HashIdDecrypted.Split("|")[1]);
                HttpContext.Session.SetObjSession("Session.CodUsr", TxtUser);
                RetData<IEnumerable<IenetGroups>> ListGroups = await ApiClientFactory.Instance.GetGroups(ApiClientFactory.Instance.Encrypt($"TopSecurity|{HashIdDecrypted.Split("|")[5]}"));
                RetData<IEnumerable<IenetAccesses>> ListAccesses = await ApiClientFactory.Instance.GetIenetAccesses(ApiClientFactory.Instance.Encrypt($"TopSecurity|{HashIdDecrypted.Split("|")[5]}"));
                RetData<IEnumerable<IenetGroupsAccesses>> ListGroupsAccesses = await ApiClientFactory.Instance.GetIEnetGroupsAccesses(ApiClientFactory.Instance.Encrypt($"TopSecurity|{HashIdDecrypted.Split("|")[5]}"));
                HttpContext.Session.SetObjSession("ListGroups", ListGroups.Data);
                HttpContext.Session.SetObjSession("ListAccesses", ListAccesses.Data);
                HttpContext.Session.SetObjSession("ListGroupsAccesses", ListGroupsAccesses.Data);
                IEnumerable<IenetGroupsAccesses> TPermit = (
                    from Ga in ListGroupsAccesses.Data
                    from A in ListAccesses.Data
                    where A.Id == Ga.IdIenetAccess
                    && Ga.IdIenetGroup == Convert.ToInt32(HashIdDecrypted.Split("|")[1])
                    && A.Descr.Equals("Login", StringComparison.OrdinalIgnoreCase)
                    select Ga
                    );
                if (TPermit.Count() > 0)
                    return LocalRedirect("/");
                else
                    return View("Index", new Models.ErrorModel() { ErrorMessage = "El usuario no tiene permiso para ingresar al sistema." });
            }
            catch (Exception e1)
            {
                return View("Index", new Models.ErrorModel() { ErrorMessage = e1.ToString().Replace("'", "") });
            }
        }
    }
}