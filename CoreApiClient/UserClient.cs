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
        public async Task<IEnumerable<Rep830Info>> GetPureEdi(string HashId = "")
        {
            return await GetAsync<IEnumerable<Rep830Info>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPureEdi"), $"HashId={HashId}"));
        }
        public async Task<IEnumerable<EdiComs>> GetEdiComs()
        {
            return await GetAsync<IEnumerable<EdiComs>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetEdiComs")));
        }
        public async Task<IEnumerable<EdiRepSent>> GetEdiRepSent()
        {
            return await GetAsync<IEnumerable<EdiRepSent>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetEdiRepSent")));
        }
        public async Task<IEnumerable<LearPureEdi>> GetLearPureEdi()
        {
            return await GetAsync<IEnumerable<LearPureEdi>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetLearPureEdi")));
        }
        public async Task<IEnumerable<TsqlDespachosWmsComplex>> GetSN(bool NoEnviado)
        {
            return await GetAsync<IEnumerable<TsqlDespachosWmsComplex>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetSN"), $"?NoEnviado={NoEnviado}"));
        }
        public async Task<IEnumerable<TsqlDespachosWmsComplex>> GetSN(IEnumerable<string> _ListDispatch, IEnumerable<string> _ListProducts)
        {
            return await GetAsync<IEnumerable<TsqlDespachosWmsComplex>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetSNDetails"), "?ListDispatch=" + string.Join('|', _ListDispatch) + "&ListProducts=" + string.Join('|', _ListProducts)));
        }
        public async Task<FE830Data> GetFE830Data(string HashId)
        {
            return await GetAsync<FE830Data>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetFE830Data"), $"HashId={HashId}"));
        }
        public async Task<string> SendForm856(IEnumerable<string> _ListDispatch, string Idusr)
        {
            return await GetAsyncNoJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SendForm856"), $"?listDispatch={string.Join('|', _ListDispatch)}&Idusr={Idusr}"));
        }
        public async Task<string> UpdateLinComments(string _LinHashId, string _TxtLinComData, string _ListFst)
        {
            return await GetAsyncNoJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/UpdateLinComments"), $"?LinHashId={_LinHashId}&TxtLinComData={_TxtLinComData}&ListFst={_ListFst}"));
        }
        public async Task<string> Login(string _User, string _Password)
        {
            return await GetAsyncNoJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/Login"), $"?User={_User}&Password={_Password}"));
        }
        public async Task<RetInfo> AutoSendInventary830(string _Force, string Idusr)
        {
            return await GetAsync<RetInfo>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Edi/AutoSendInventary830"), $"?Force={_Force}&Idusr={Idusr}"));
        }
        public async Task<string> LastRep()
        {
            return await GetAsyncNoJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/LastRep")));
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
