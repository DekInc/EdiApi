using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
//using System.Diagnostics;

namespace EdiViewer.Utility.Scheduling
{
    public class AutoSendInventary830Task : Interfaces.IScheduledTask
    {
        public string Schedule => "*/5 * * * *";
        public static HttpClient httpClient = new HttpClient();        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            string AutoSendInventary830Uri = $"{ApplicationSettings.ApiUri}Edi/AutoSendInventary830";
            string LastRepUri = $"{ApplicationSettings.ApiUri}Data/LastRep";
            try
            {
                //Debug.WriteLine("ejecutado!!! " + DateTime.Now.ToString());
                //System.IO.StreamWriter Sw2 = new System.IO.StreamWriter(@"c:\temp\EdiLog.txt", true);
                //Sw2.WriteLine("EdiTask init" + DateTime.Now.ToString() + Environment.NewLine);
                //Sw2.Close();
                if ((DateTime.Now.DayOfWeek == DayOfWeek.Friday
                    || DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                    || DateTime.Now.DayOfWeek == DayOfWeek.Sunday
                    )
                    && DateTime.Now.Hour > 18)
                {
                    Uri Url = new Uri(LastRepUri);
                    string LastRep = await httpClient.GetStringAsync(Url);
                    if (string.IsNullOrEmpty(LastRep))
                    {
                        Uri Url2 = new Uri(AutoSendInventary830Uri);
                        string Res = await httpClient.GetStringAsync(Url2);
                    }
                    else {
                        DateTime LastDateRep = LastRep.ToDate();
                        if ((DateTime.Now - LastDateRep).TotalDays > 4) {
                            Uri Url2 = new Uri(AutoSendInventary830Uri);
                            string Res = await httpClient.GetStringAsync(Url2);
                        }
                    }
                }
                //try
                //{
                //    System.IO.StreamWriter Sw = new System.IO.StreamWriter(@"c:\temp\EdiLog.txt", true);
                //    Sw.WriteLine(ResJson + DateTime.Now.ToString() + Environment.NewLine);
                //    Sw.Close();
                //}
                //catch { }
            }
            catch (Exception e1) { }
        }
    }
}
