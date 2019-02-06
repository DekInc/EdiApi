using EdiApi.Models.EdiDB;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class ComRepoMail : Coms
    {
        private const string Id = "IMAP";        
        public static StreamReader GetEdi830File(string _IMapHost, int _IMapPortIn, int _IMapPortOut, string _IMapUser, string _IMapPassword, bool _IMapSSL, ref int _CodError, ref string _MessageSubject, ref string _FileName, ref EdiDBContext _DbO, object _MaxEdiComs)
        {
            CheckMaxEdiComs(ref _DbO, _MaxEdiComs);
            using (ImapClient ImapClientO = new ImapClient(_IMapHost, _IMapPortIn, _IMapUser, _IMapPassword, AuthMethod.Login, _IMapSSL))
            {
                IEnumerable<uint> uids = ImapClientO.Search(SearchCondition.Unseen());
                IEnumerable<MailMessage> ArrMessages = ImapClientO.GetMessages(uids);
                if (ArrMessages.Count() > 0)
                {
                    MailMessage MailMessageO = ArrMessages.LastOrDefault();
                    _MessageSubject = MailMessageO.Subject;
                    if (MailMessageO.Attachments.Count > 0)
                    {
                        _FileName = MailMessageO.Attachments.FirstOrDefault().Name;
                        AddComLog(ref _DbO, Id, $"Se obtuvo el archivo {_FileName}");
                        return new StreamReader(MailMessageO.Attachments.FirstOrDefault().ContentStream);
                    }
                    else
                    {
                        AddComLog(ref _DbO, Id, $"Error, el correo verificado no contiene ningún archivo. Subject = {_MessageSubject}.");
                        _CodError = -1;
                    }
                }
                else
                {
                    AddComLog(ref _DbO, Id, $"No hay correos a verificar.");
                    _CodError = -2;
                }
            }
            return null;
        }
        public static void GetEdi830File(string _IMapHost, int _IMapPortIn, int _IMapPortOut, string _IMapUser, string _IMapPassword, bool _IMapSSL, ref int _CodError, ref string _MessageSubject, ref string _FileName, ref EdiDBContext _DbO, object _MaxEdiComs, ref List<string> _EdiPure)
        {
            CheckMaxEdiComs(ref _DbO, _MaxEdiComs);
            try
            {
                using (ImapClient ImapClientO = new ImapClient(_IMapHost, _IMapPortIn, _IMapUser, _IMapPassword, AuthMethod.Login, _IMapSSL))
                {
                    IEnumerable<uint> uids = ImapClientO.Search(SearchCondition.Unseen());
                    IEnumerable<MailMessage> ArrMessages = ImapClientO.GetMessages(uids);
                    if (ArrMessages.Count() > 0)
                    {
                        MailMessage MailMessageO = ArrMessages.LastOrDefault();
                        _MessageSubject = MailMessageO.Subject;
                        if (MailMessageO.Attachments.Count > 0)
                        {
                            _FileName = MailMessageO.Attachments.FirstOrDefault().Name;
                            AddComLog(ref _DbO, Id, $"Se obtuvo el archivo {_FileName}");
                            StreamReader Rep830File = new StreamReader(MailMessageO.Attachments.FirstOrDefault().ContentStream);
                            while (!Rep830File.EndOfStream)
                            {
                                _EdiPure.Add(Rep830File.ReadLine());
                            }
                            Rep830File.Close();
                        }
                        else
                        {
                            AddComLog(ref _DbO, Id, $"Error, el correo verificado no contiene ningún archivo. Subject = {_MessageSubject}.");
                            _CodError = -1;
                        }
                    }
                    else
                    {
                        AddComLog(ref _DbO, Id, $"No hay correos a verificar.");
                        _CodError = -2;
                    }
                }
            }
            catch (Exception Me1)
            {
                AddComLog(ref _DbO, Id, Me1.ToString());
                throw Me1;
            }            
        }
    }
}
