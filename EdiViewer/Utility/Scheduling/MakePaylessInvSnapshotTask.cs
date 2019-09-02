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
    public class MakePaylessInvSnapshotTask : Interfaces.IScheduledTask
    {
        public string Schedule => "*/20 * * * *";
        public static HttpClient httpClient = new HttpClient();        
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            string MakePaylessInvSnapshotTaskUri = $"{ApplicationSettings.ApiUri}Data/MakePaylessInvSnapshot?ClienteId=1432";
            try
            {
                Uri Url = new Uri(MakePaylessInvSnapshotTaskUri);
                string ResJson = await httpClient.GetStringAsync(Url);
            }
            catch { }

            MakePaylessInvSnapshotTaskUri = $"{ApplicationSettings.ApiUri}Data/MakePaylessInvSnapshot?ClienteId=385";
            try {
                Uri Url = new Uri(MakePaylessInvSnapshotTaskUri);
                string ResJson = await httpClient.GetStringAsync(Url);
            } catch { }
        }
    }    
}