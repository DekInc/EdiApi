using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComModels;

namespace CoreApiClient
{
    public partial class ApiClient
    {
        public async Task<RetReporte> TranslateForms830()
        {
            return await GetAsync<RetReporte>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Edi/TranslateForms830")));
        }
        public async Task<Rep830Info> GetPureEdi(string HashId = "")
        {
            return await GetAsync<Rep830Info>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPureEdi"), $"HashId={HashId}"));
        }

        public async Task<FE830Data> GetFE830Data(string HashId)
        {
            return await GetAsync<FE830Data>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetFE830Data"), $"HashId={HashId}"));
        }

        //public async Task<List<UsersModel>> GetUsers()
        //{
        //    var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
        //        "User/GetAllUsers"));
        //    return await GetAsync<List<UsersModel>>(requestUrl);
        //}

        //public async Task<Message<UsersModel>> SaveUser(UsersModel model)
        //{
        //    var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
        //        "User/SaveUser"));
        //    return await PostAsync<UsersModel>(requestUrl, model);
        //}
    }
}
