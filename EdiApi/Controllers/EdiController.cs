using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiApi.Models;

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
                LearIsa LearIsaO = DbO.LearIsa.Where(I => I.Id == Tipo).FirstOrDefault();
                //if (LearIsaO == null)
                //{
                //    return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = "Tipo de reporte inválido." } };
                //}
                LearRep830 Rep830O = new LearRep830(0, 1, LearIsaO);
                //return new RetReporte() { EdiFile = Rep830O.ToString() };
                return Rep830O.ToString();
            }
            catch (Exception e1)
            {
                return "Error";
                //return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = e1.ToString() } };
            }            
        }

        [HttpGet]
        //[Route("~/Edi/Form856")]
        public ActionResult<IEnumerable<string>> Form856()
        {            
            return new string[] { "value5", "value4" };
        }
        [HttpGet]
        //[Route("~/Edi/CrearDB")]
        public ActionResult<string> CrearDB()
        {
            LearIsa LN = new LearIsa()
            {
                AuthorizationInformationQualifier = "00",
                AuthorizationInformation = "          ",
                SecurityInformationQualifier = "00",
                SecurityInformation = "          ",
                InterchangeSenderIdQualifier = "ZZ",
                InterchangeSenderId = "GLC503",
                InterchangeReceiverIdQualifier = "ZZ",
                InterchangeReceiverId = "HN02NC72       ",
                InterchangeControlStandardsId = "U",
                InterchangeControlVersion = "00204",
                AcknowledgmentRequested = "0",
                UsageIndicator = "T", // T o P
                ComponentElementSeparator = "<",
            };
            DbO.LearIsa.Add(LN);
            DbO.SaveChanges();
            return "";
        }
    }
}
