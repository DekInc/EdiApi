using CoreApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdiViewer
{
    internal static class ApiClientFactory
    {
        private static Uri apiUri;
        private static Lazy<ApiClient> restClient = new Lazy<ApiClient>(
          () => new ApiClient(apiUri, false),
          LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiClientFactory()
        {
            apiUri = new Uri(ApplicationSettings.ApiUri);
        }

        public static ApiClient Instance {
            get { return restClient.Value; }
        }
    }
    internal static class ApiLongClientFactory {
        private static Uri apiUri;
        private static Lazy<ApiClient> restClient = new Lazy<ApiClient>(
          () => new ApiClient(apiUri, true),
          LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiLongClientFactory() {
            apiUri = new Uri(ApplicationSettings.ApiUri);
        }

        public static ApiClient Instance {
            get { return restClient.Value; }
        }
    }
}
