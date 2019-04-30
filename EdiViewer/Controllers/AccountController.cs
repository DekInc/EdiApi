﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreApiClient;
using System.Diagnostics;
using System.Text;
using System.Net;
using ComModels;
using EdiViewer.Models;
using Microsoft.AspNetCore.Http;

namespace EdiViewer.Controllers
{
    public class AccountController : Controller
    {
        private void CheckSession() {
            if (string.IsNullOrEmpty(HttpContext.Session.GetObjSession<string>("Session.HashId"))) {
                Response.Redirect("/Account/?error=NO_AUTH");
            }
        }

        public IActionResult Index()
        {
            return View(new Models.ErrorModel());
        }
        public IActionResult CrudUsuarios()
        {
            CheckSession();
            return View();
        }
        public IActionResult CrudGrupos()
        {
            CheckSession();
            return View();
        }
        public IActionResult CrudAccesos()
        {
            CheckSession();
            return View();
        }
        public IActionResult CrudGrupoAccesos()
        {
            CheckSession();
            return View();
        }        
        [HttpGet]
        public bool MiAlive()
        {
            string CodUsr = HttpContext.Session.GetObjSession<string>("Session.CodUsr");
            if (string.IsNullOrEmpty(CodUsr)) return false;
            return true;
        }
        public async Task<IActionResult> GetUsers()
        {
            try
            {                
                RetData<IEnumerable<IenetUsers>> ListUsers = await ApiClientFactory.Instance.GetUsers(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));                
                IEnumerable<IenetGroups> ListGroups = HttpContext.Session.GetObjSession<IEnumerable<IenetGroups>>("ListGroups");
                RetData<IEnumerable<Clientes>> ListClients = await ApiClientFactory.Instance.GetAllClients(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));                
                List<IenetUsersModel> Records = new List<IenetUsersModel>();
                List<IenetUsersModel> AllRecords = new List<IenetUsersModel>();
                foreach (IenetUsers UserO in ListUsers.Data)
                {
                    Records.Add(new IenetUsersModel() {
                        Id = UserO.Id,
                        CodUsr = UserO.CodUsr,
                        IdIenetGroup = UserO.IdIenetGroup,
                        NomUsr = UserO.NomUsr,
                        ClienteId = UserO.ClienteId,
                        Cliente = (UserO.ClienteId.HasValue? ListClients.Data.Where(C => C.ClienteId == UserO.ClienteId).Fod().Nombre : ""),
                        IenetGroup = ListGroups.Where(G => G.Id == UserO.IdIenetGroup).Fod().Descr
                    });
                }
                int Total = 0;
                if (Records.Count > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<IenetUsersModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<IenetUsersModel>(Records, Request.Form);
                    Total = Records.Count;
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            }
            catch (Exception e1)
            {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetGroups()
        {
            try
            {                
                RetData<IEnumerable<IenetGroups>> ListGroups = await ApiClientFactory.Instance.GetGroups(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                List<IenetGroupsModel> Records = ListGroups.Data.Select(O => new IenetGroupsModel() { Id = O.Id, Descr = O.Descr }).ToList();
                List<IenetGroupsModel> AllRecords = new List<IenetGroupsModel>();
                int Total = 0;
                if (ListGroups.Data.Count() > 0)
                {   
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<IenetGroupsModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<IenetGroupsModel>(Records, Request.Form);
                    Total = Records.Count;
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            }
            catch (Exception e1)
            {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetAccesss()
        {
            try
            {
                RetData<IEnumerable<IenetAccesses>> ListData = await ApiClientFactory.Instance.GetIenetAccesses(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                List<IenetAccessesModel> Records = ListData.Data.Select(O => new IenetAccessesModel() { Id = O.Id, Descr = O.Descr }).ToList();
                List<IenetAccessesModel> AllRecords = new List<IenetAccessesModel>();
                int Total = 0;
                if (Records.Count() > 0)
                {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<IenetAccessesModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<IenetAccessesModel>(Records, Request.Form);
                    Total = Records.Count;
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            }
            catch (Exception e1)
            {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetGroupsAccesss()
        {
            try
            {
                RetData<IEnumerable<IenetGroups>> ListGroups = await ApiClientFactory.Instance.GetGroups(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                RetData<IEnumerable<IenetAccesses>> ListAccess = await ApiClientFactory.Instance.GetIenetAccesses(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                RetData<IEnumerable<IenetGroupsAccesses>> ListGroupsAccesses = await ApiClientFactory.Instance.GetIEnetGroupsAccesses(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                List<IenetGroupsAccessesModel> AllRecords = new List<IenetGroupsAccessesModel>();
                List<IenetGroupsAccessesModel> Records = (
                    from Ga in ListGroupsAccesses.Data
                    from G in ListGroups.Data
                    from A in ListAccess.Data
                    where Ga.IdIenetAccess == A.Id
                    && Ga.IdIenetGroup == G.Id
                    select new IenetGroupsAccessesModel() {
                        Id = Ga.Id,
                        IdIenetAccess = A.Id,
                        IdIenetGroup = G.Id,
                        Access = A.Descr,
                        Group = G.Descr
                    }).ToList();
                int Total = 0;
                if (Records.Count() > 0)
                {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<IenetGroupsAccessesModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<IenetGroupsAccessesModel>(Records, Request.Form);
                    Total = Records.Count;
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            }
            catch (Exception e1)
            {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
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