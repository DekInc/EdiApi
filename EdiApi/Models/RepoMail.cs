using S22.Imap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public static class RepoMail
    {
        public static StreamReader GetEdi830File(string _IMapHost, int _IMapPortIn, int _IMapPortOut, string _IMapUser, string _IMapPassword, bool _IMapSSL, ref int _CodError, ref string _MessageSubject)
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
                        return new StreamReader(MailMessageO.Attachments.FirstOrDefault().ContentStream);
                    }
                    else _CodError = -1;
                }
                else _CodError = -2;
            }
            return null;
        }
    }
}
