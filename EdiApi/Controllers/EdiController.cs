using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiApi.Models;
using System.IO;
using System.Net.Mail;
using S22.Imap;
using Microsoft.Extensions.Configuration;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EdiController : ControllerBase
    {
        public readonly EdiDBContext DbO;
        public readonly IConfiguration Config;
        IConfiguration IMapConfig => Config.GetSection("IMapConfig");
        string IMapHost => (string)IMapConfig.GetValue(typeof(string), "Host");
        int IMapPort => Convert.ToInt32(IMapConfig.GetValue(typeof(string), "Port"));
        bool IMapSSL => Convert.ToBoolean(IMapConfig.GetValue(typeof(string), "SSL"));
        string IMapUser => (string)IMapConfig.GetValue(typeof(string), "User");
        string IMapPassword => (string)IMapConfig.GetValue(typeof(string), "Password");
        //public EdiController(EdiDBContext _DbO) { DbO = _DbO; }
        public EdiController(EdiDBContext _DbO, IConfiguration _Config) { DbO = _DbO; Config = _Config; }        

        [HttpGet]
        //De modo que se expone https://localhost:44373/Edi/Form830
        public ActionResult<RetReporte> Form830()
        {
            try
            {                
                LearRep830 LearRep830O = new LearRep830();
                try
                {
                    using (ImapClient ImapClientO = new ImapClient(IMapHost, IMapPort, IMapUser, IMapPassword, AuthMethod.Login, IMapSSL))
                    {
                        IEnumerable<uint> uids = ImapClientO.Search(SearchCondition.Unseen());
                        IEnumerable<MailMessage> ArrMessages = ImapClientO.GetMessages(uids);
                        if (ArrMessages.Count() > 0)
                        {
                            MailMessage MailMessageO = ArrMessages.LastOrDefault();
                            if (MailMessageO.Attachments.Count > 0)
                            {
                                StreamReader Rep830File = new StreamReader(MailMessageO.Attachments.FirstOrDefault().ContentStream);
                                while (!Rep830File.EndOfStream)
                                    LearRep830O.EdiFile.Add(Rep830File.ReadLine());
                                Rep830File.Close();                                
                            }
                            else return new RetReporte() { Info = new RetInfo() { CodError = -1, Mensaje = $"Error, el correo verificado no contiene ningún archivo. Subject = {MailMessageO.Subject}." } };
                        }
                        else return new RetReporte() { Info = new RetInfo() { CodError = -2, Mensaje = $"Error, no hay correos a verificar." } };
                    }
                }
                catch (Exception ExMail)
                {
                    return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = ExMail.ToString() } };
                }
                LearRep830O.Parse();
                return new RetReporte() { EdiFile = string.Join("~", LearRep830O.EdiFile), Info = new RetInfo() { CodError = 0, Mensaje = "Todo OK" } };
            }
            catch (Exception e1)
            {
                return new RetReporte() { Info = new RetInfo() { CodError = 1, Mensaje = e1.ToString() } };
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
        //[Route("~/Edi/Form856")]
        public ActionResult<string> EnviarMail()
        {   
            using (ImapClient Client = new ImapClient(IMapHost, IMapPort, IMapUser, IMapPassword, AuthMethod.Login, IMapSSL))
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("EdiTest@dnecr.ml")
                };
                message.To.Add("KeyFireOne@gmail.com");
                message.Subject = "EdiTest_Writting_5";
                message.Body = "bla bla bla 5";
                //Attachment attachment = new Attachment("some_image.png", "image/png");
                //attachment.Name = "my_attached_image.png";
                //message.Attachments.Add(attachment);
                uint uid = Client.StoreMessage(message);
                //Console.WriteLine("The UID of the stored mail message is " + uid);
            }
            return "Done";
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
