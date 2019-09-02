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
    public class MakeAutoReportsPaylessTask : Interfaces.IScheduledTask
    {
        public string Schedule => "*/30 * * * *";
        public static HttpClient httpClient = new HttpClient();        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            string MakeAutoReportsPaylessTaskUri = $"{ApplicationSettings.ApiUri}Data/MakeAutoReportsPayless?ClienteId=1432";
            try
            {                
                Uri Url = new Uri(MakeAutoReportsPaylessTaskUri);
                string ResJson = await httpClient.GetStringAsync(Url);
            }
            catch { }
        }
    }    
}