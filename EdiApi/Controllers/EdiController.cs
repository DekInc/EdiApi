﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiApi.Models;
using System.IO;
using System.Net.Mail;
using S22.Imap;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using EdiApi.Models.EdiDB;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EdiController : ControllerBase
    {
        public EdiDBContext DbO;
        public EdiDBContext DbOEx;
        private Models.WmsDB.WmsContext WmsDb;
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
        StreamReader Rep830File { set; get; }
        //public EdiController(EdiDBContext _DbO) { DbO = _DbO; }
        public EdiController(EdiDBContext _DbO, Models.WmsDB.WmsContext _WmsDb, IConfiguration _Config) { DbO = _DbO; WmsDb = _WmsDb; Config = _Config; }
        [HttpGet]
        public ActionResult<RetReporte> EnviarEjemplo()
        {
            DateTime StartTime = DateTime.Now;
            ComRepoFtp ComRepoFtpO = new ComRepoFtp(
                        (string)IEdiFtpConfig.GetValue(typeof(string), "Host"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "HostFailover"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "EdiUser"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "EdiPassword"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirIn"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirOut"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirChecked"),
                        Config.GetSection("MaxEdiComs").GetValue(typeof(string), "Value")
                    );
            if (!ComRepoFtpO.Ping(ref DbO))
            {
                ComRepoFtpO.UseHost2 = true;
                if (!ComRepoFtpO.Ping(ref DbO))
                {
                    return new RetReporte()
                    {
                        Info = new RetInfo()
                        {
                            CodError = -3,
                            Mensaje = $"Error, no se puede conectar con el servidor FTP primario o secundario",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            ComRepoFtpO.Put("AEnviar.txt", @"C:\temp\AEnviar.txt", ref DbO);
            return new RetReporte()
            {
                Info = new RetInfo()
                {
                    CodError = 0,
                    Mensaje = "Todo ok",
                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                }
            };
        }

        [HttpGet]
        //De modo que se expone https://localhost:44373/Edi/TranslateForms830
        public ActionResult<RetReporte> TranslateForms830()
        {
            //System.IO.StreamWriter Sw2 = new System.IO.StreamWriter(@"c:\temp\EdiLog.txt", true);
            //Sw2.WriteLine("TranslateForms830 init" + DateTime.Now.ToString() + Environment.NewLine);
            //Sw2.Close();
            DateTime StartTime = DateTime.Now;
            LearRep830 LearRep830O = new LearRep830(ref DbO);
            try
            {   
                int CodError2 = 0;
                string MessageSubject = string.Empty, FileName = string.Empty, EdiPureStr = string.Empty;
                List<string> ListEdiPure = new List<string>();
                try
                {
                    //StreamReader Rep830File = ComRepoMail.GetEdi830File(IMapHost, IMapPortIn, IMapPortOut, IMapUser, IMapPassword, IMapSSL, ref CodError, ref MessageSubject, ref FileName, ref DbO, Config.GetSection("MaxEdiComs").GetValue(typeof(string), "Value"));
                    ComRepoFtp ComRepoFtpO = new ComRepoFtp(
                        (string)IEdiFtpConfig.GetValue(typeof(string), "Host"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "HostFailover"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "EdiUser"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "EdiPassword"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirIn"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirOut"),
                        (string)IEdiFtpConfig.GetValue(typeof(string), "DirChecked"),
                        Config.GetSection("MaxEdiComs").GetValue(typeof(string), "Value")
                    );
                    if (!ComRepoFtpO.Ping(ref DbO))
                    {
                        ComRepoFtpO.UseHost2 = true;
                        if (!ComRepoFtpO.Ping(ref DbO))
                        {
                            return new RetReporte()
                            {
                                Info = new RetInfo()
                                {
                                    CodError = -3,
                                    Mensaje = $"Error, no se puede conectar con el servidor FTP primario o secundario",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                    }
                    ComRepoFtpO.Get(ref DbO, ref CodError2, ref MessageSubject, ref FileName, ref ListEdiPure);
                    //ComRepoMail.GetEdi830File(IMapHost, IMapPortIn, IMapPortOut, IMapUser, IMapPassword, IMapSSL, ref CodError2, ref MessageSubject, ref FileName, ref DbO, Config.GetSection("MaxEdiComs").GetValue(typeof(string), "Value"), ref ListEdiPure);
                    switch (CodError2)
                    {
                        case -1:
                            return new RetReporte() {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = $"Error, el correo verificado no contiene ningún archivo. Subject = {MessageSubject}.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        case -2:                            
                            return new RetReporte() {
                                Info = new RetInfo() {
                                    CodError = -2,
                                    Mensaje = $"Error, no hay nada a verificar.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        case -4:
                        case -5:
                        case -6:
                        case -7:
                        case -8:
                        case -9:
                        case -10:
                        case -11:
                            return new RetReporte()
                            {
                                Info = new RetInfo()
                                {
                                    CodError = CodError2,
                                    Mensaje = MessageSubject,
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                    }
                    for (int Ifn = 0; Ifn < ListEdiPure.Count; Ifn++)
                        ListEdiPure[Ifn] = ListEdiPure[Ifn].Replace(Environment.NewLine, "");
                    if (ListEdiPure.Count > 1)
                    {
                        ListEdiPure.ForEach(E => EdiPureStr += E);
                        if (ListEdiPure[1].Contains(EdiBase.SegmentTerminator))
                        {                            
                            LearRep830O.EdiFile = EdiPureStr.Split(EdiBase.SegmentTerminator).ToList();
                        }
                        else
                        {
                            LearRep830O.EdiFile = ListEdiPure;
                        }
                    }
                    else if(ListEdiPure.Count == 1) {
                        if (ListEdiPure.FirstOrDefault().Contains(EdiBase.SegmentTerminator))
                        {
                            LearRep830O.EdiFile = ListEdiPure.FirstOrDefault().Split(EdiBase.SegmentTerminator).ToList();
                        }
                        else {
                            return new RetReporte()
                            {
                                Info = new RetInfo()
                                {
                                    CodError = -13,
                                    Mensaje = $"El archivo Edi {FileName} no contiene un separador de segmento válido, no se puede procesar.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                    } else
                    {
                        return new RetReporte()
                        {
                            Info = new RetInfo()
                            {
                                CodError = -14,
                                Mensaje = $"El archivo Edi {FileName} no contiene ninguna linea de contenido.",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    }
                    if (LearRep830O.EdiFile.LastOrDefault() == "") LearRep830O.EdiFile.RemoveAt(LearRep830O.EdiFile.Count - 1);
                }
                catch (Exception ExMail)
                {
                    return new RetReporte() {
                        Info = new RetInfo() {
                            CodError = 1,
                            Mensaje = ExMail.ToString(),
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                LearRep830O.SaveEdiPure(ref EdiPureStr, FileName, LearRep830O.EdiFile.Count);
                string ParseRet = LearRep830O.Parse();
                if (!string.IsNullOrEmpty(ParseRet))
                {
                    LearRep830O.LearPureEdiO.Reprocesar = false;
                    LearRep830O.LearPureEdiO.Fprocesado = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat);
                    LearRep830O.LearPureEdiO.Log = ParseRet;
                    DbO.LearPureEdi.Update(LearRep830O.LearPureEdiO);
                    DbO.SaveChanges();
                    return new RetReporte()
                    {
                        Info = new RetInfo()
                        {
                            CodError = -12,
                            Mensaje = ParseRet,
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                LearRep830O.SaveAll();
                if (LearRep830.LearIsa830root != null)
                {
                    LearRep830.LearIsa830root = DbO.LearIsa830.Where(L => L.HashId == LearRep830.LearIsa830root.HashId).FirstOrDefault();
                    LearRep830.LearIsa830root.ParentHashId = LearRep830O.LearPureEdiO.HashId;
                    DbO.LearIsa830.Update(LearRep830.LearIsa830root);
                }                
                LearRep830O.UpdateEdiPure();
                return new RetReporte() {
                    EdiFile = string.Join(EdiBase.SegmentTerminator, LearRep830O.EdiFile),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "Todo OK",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                try
                {
                    LearRep830O.LearPureEdiO.Log = e1.ToString();
                    DbContextOptionsBuilder<EdiDBContext> optionsBuilder = new DbContextOptionsBuilder<EdiDBContext>();
                    optionsBuilder.UseSqlServer(Config.GetConnectionString("EdiDB"));
                    DbOEx = new EdiDBContext(optionsBuilder.Options);                    
                    DbOEx.LearPureEdi.Update(LearRep830O.LearPureEdiO);
                    DbOEx.SaveChanges();
                }
                catch (Exception SevereEx)
                {
                    return new RetReporte() {
                        Info = new RetInfo() {
                            CodError = 1,
                            Mensaje = "ERROR GRAVE DE BASE DE DATOS. " + SevereEx.ToString(),
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                return new RetReporte() {
                    Info = new RetInfo() {
                        CodError = 1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }            
        }
        private string SendEdiFtp(string _EdiStr)
        {
            ComRepoFtp ComRepoFtpO = new ComRepoFtp(
                        FtpHost,
                        FtpHostFailover,
                        FtpUser,
                        FtpPassword,
                        FtpDirIn,
                        FtpDirOut,
                        FtpDirChecked,
                        MaxEdiComs);
            if (!ComRepoFtpO.Ping(ref DbO))
            {
                ComRepoFtpO.UseHost2 = true;
                if (!ComRepoFtpO.Ping(ref DbO))
                {
                    return "Error, no se puede conectar con el servidor FTP primario o secundario";
                }
            }
            ComRepoFtpO.Put(_EdiStr, ref DbO);
            return "ok";
        }
        public ActionResult<RetInfo> AutoSendInventary830()
        {
            DateTime StartTime = DateTime.Now;
            try
            {   
                LearRep830 LearRep830O = new LearRep830(ref DbO, ref WmsDb);
                SendEdiFtp(LearRep830O.AutoSendInventary830());
                return new RetInfo()
                {
                    CodError = 0,
                    Mensaje = "ok",
                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                };
            }
            catch (Exception e1)
            {
                return new RetInfo()
                {
                    CodError = -1,
                    Mensaje = e1.ToString(),
                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                };
            }            
        }
    }
}
