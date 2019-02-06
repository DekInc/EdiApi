using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EdiViewer.Utility.Scheduling
{
    public class GetEdi830Task : Interfaces.IScheduledTask
    {
        public string Schedule => "*/30 * * * *";
        public static HttpClient httpClient = new HttpClient();
        private static readonly string Transform830Uri = $"{ApplicationSettings.ApiUri}Edi/TranslateForms830";
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Uri Url = new Uri(Transform830Uri);
            string ResJson = await httpClient.GetStringAsync(Url);
            try
            {
                System.IO.StreamWriter Sw = new System.IO.StreamWriter(@"c:\temp\EdiLog.txt", true);
                Sw.WriteLine(ResJson + Environment.NewLine);
                Sw.Close();
            }
            catch{}
        }
    }    
}