﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiApi.Models;
using System.IO;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EdiController : ControllerBase
    {
        public readonly EdiDBContext DbO;
        public EdiController(EdiDBContext _DbO) { DbO = _DbO; }
        [HttpGet]
        //De modo que se expone https://localhost:44373/Edi/Form830
        public ActionResult<string> Form830(int Tipo)
        {
            try
            {
                //LearIsa LearIsaO = DbO.LearIsa
                //    .Where(I => I.Id == Tipo)
                //    .OrderByDescending(Io => Io.Id).LastOrDefault();
                //LearGs LearGsO = DbO.LearGs
                //    .Where(G => G.IdIsa == Tipo)
                //    .OrderByDescending(Go => Go.Id).LastOrDefault();
                //if (LearIsaO == null || LearGsO == null)
                //{
                //    return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = "Tipo de reporte inválido." } };
                //}
                //LearBfr LearBfr830O = new LearBfr
                //{
                //    IdGs = LearGsO.Id
                //};
                //DbO.LearBfr.Add(LearBfr830O);
                //DbO.SaveChanges();

                //LearRep830 Rep830O = new LearRep830(0, LearBfr830O.Id, ref LearIsaO, ref LearGsO, ref LearBfr830O);

                //DbO.LearBfr.Update(LearBfr830O);
                //DbO.SaveChanges();
                //return new RetReporte() { EdiFile = Rep830O.ToString() };
                //return Rep830O.ToString();
                LearRep830 LearRep830O = new LearRep830();
                StreamReader Rep830File = new StreamReader("830_ejemplo.txt");
                while (!Rep830File.EndOfStream)
                    LearRep830O.EdiFile.Add(Rep830File.ReadLine());
                Rep830File.Close();
                return LearRep830O.Parse();
            }
            catch (Exception e1)
            {
                return "Error: " + e1.ToString();
                //return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = e1.ToString() } };
            }            
        }

        [HttpGet]
        //[Route("~/Edi/Form856")]
        public ActionResult<IEnumerable<string>> Form856()
        {
            LearRep856 LearRep856O = new LearRep856();
            StreamReader Rep856File = new StreamReader("F856_048700342.txt");
            while (!Rep856File.EndOfStream)
                LearRep856O.EdiFile.Add(Rep856File.ReadLine());
            Rep856File.Close();
            return new string[] { "value5", "value4" };
        }
        [HttpGet]
        //[Route("~/Edi/CrearDB")]
        public ActionResult<string> CrearDB()
        {
            //LearIsa LN = new LearIsa()
            //{
            //    AuthorizationInformationQualifier = "00",
            //    AuthorizationInformation = "          ",
            //    SecurityInformationQualifier = "00",
            //    SecurityInformation = "          ",
            //    InterchangeSenderIdQualifier = "ZZ",
            //    InterchangeSenderId = "GLC503",
            //    InterchangeReceiverIdQualifier = "ZZ",
            //    InterchangeReceiverId = "HN02NC72       ",
            //    InterchangeControlStandardsId = "U",
            //    InterchangeControlVersion = "00204",
            //    AcknowledgmentRequested = "0",
            //    UsageIndicator = "T", // T o P
            //    ComponentElementSeparator = "<",
            //    InterchangeDate = "yyMMdd",
            //    InterchangeTime = "HHmm"
            //};
            //DbO.LearIsa.Add(LN);
            //LearGs LearGsO = new LearGs() {
            //    FunctionalIdCode = "SH",
            //    ApplicationSenderCode = "GLC503",
            //    ApplicationReceiverCode = "HN02NC72",
            //    GsDate = "yyMMdd",
            //    GsTime = "HHmm",
            //    ResponsibleAgencyCode = "X",
            //    Version = "002040",
            //    IdIsa = 1
            //};
            //DbO.LearGs.Add(LearGsO);
            //DbO.SaveChanges();
            return "";
        }
    }
}
