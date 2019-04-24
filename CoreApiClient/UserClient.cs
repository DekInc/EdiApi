using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComModels;
using Newtonsoft.Json;

namespace CoreApiClient
{
    public partial class ApiClient
    {
        public string Encrypt(string Val) {
            return Convert.ToBase64String(CryptoHelper.EncryptData(Encoding.UTF8.GetBytes(Val)));
        }
        //public byte[] Decrypt(byte[] Val)
        //{
        //    return CryptoHelper.DecryptData(Val);
        //}
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
        // Extranet
        public async Task<string> LoginExtern(string _User, string _Password)
        {
            UserModel UserO = new UserModel()
            {
                User = _User,
                Password = _Password
            };
            string JsonParams = JsonConvert.SerializeObject(UserO);
            return await PostAsyncJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/LoginExtern")), JsonParams);
        }        
        public async Task<RetData<IEnumerable<ExistenciasExternModel>>> GetStock(int ClientId)
        {            
            string JsonParams = JsonConvert.SerializeObject(ClientId);
            string JsonRes = await PostAsyncJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetStock")), JsonParams);
            return JsonConvert.DeserializeObject<RetData<IEnumerable<ExistenciasExternModel>>>(JsonRes);
        }
        public async Task<RetData<Clientes>> GetClient(int ClientId)
        {
            string JsonParams = JsonConvert.SerializeObject(ClientId);
            string JsonRes = await PostAsyncJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClient")), JsonParams);
            return JsonConvert.DeserializeObject<RetData<Clientes>>(JsonRes);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternos(int ClienteId)
        {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternos")), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>> GetPedidosExternosPendientes()
        {
            return await GetAsync<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosPendientes")));
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosAdmin(int PedidoId)
        {
            string JsonParams = JsonConvert.SerializeObject(PedidoId);
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosAdmin")), JsonParams);
        }
        public async Task<RetData<IEnumerable<PedidosDetExternos>>> GetPedidosDetExternos(int PedidoId)
        {
            string JsonParams = JsonConvert.SerializeObject(PedidoId);
            return await PostGetAsyncJson<RetData<IEnumerable<PedidosDetExternos>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosDetExternos")), JsonParams);
        }
        public async Task<RetData<PedidosExternos>> SetPedidoExterno(IEnumerable<PedidoExternoModel> ListDis, int ClienteId, int IdEstado)
        {            
            string JsonParams = JsonConvert.SerializeObject(ListDis);
            return await PostGetAsyncJson<RetData<PedidosExternos>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPedidoExterno"), $"?ClienteId={ClienteId}&IdEstado={IdEstado}"), JsonParams);
        }
        public async Task<RetData<PedidosDetExternos>> SetPedidoDetExterno(PedidosDetExternos PedidoDetExterno)
        {
            string JsonParams = JsonConvert.SerializeObject(PedidoDetExterno);
            return await PostGetAsyncJson<RetData<PedidosDetExternos>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPedidoDetExterno")), JsonParams);
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetPedidosWms(int ClienteId)
        {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await PostGetAsyncJson<RetData<IEnumerable<PedidosWmsModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosWms")), JsonParams);
        }
        public async Task<RetData<IEnumerable<ClientesModel>>> GetClientsOrders()
        {
            return await GetAsync<RetData<IEnumerable<ClientesModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClientsOrders")));
        }
        public async Task<RetData<IEnumerable<TsqlDespachosWmsComplex>>> GetPedidosDet(int PedidoId)
        {
            string JsonParams = JsonConvert.SerializeObject(PedidoId);
            return await PostGetAsyncJson<RetData<IEnumerable<TsqlDespachosWmsComplex>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosDet")), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>>> GetPaylessProdPriori(string Period)
        {
            string JsonParams = JsonConvert.SerializeObject(Period);
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessProdPriori")), JsonParams);
        }
        public async Task<RetData<string>> SetPaylessProdPriori(IEnumerable<PaylessUploadFileModel> ListUpload, int ClienteId, string Periodo, string codUsr, string transporte)
        {
            string JsonParams = JsonConvert.SerializeObject(ListUpload);
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPaylessProdPriori"), $"?ClienteId={ClienteId}&Periodo={Periodo}&codUsr={codUsr}&transporte={transporte}"), JsonParams);
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPriori()
        {
            return await GetAsync<RetData<IEnumerable<string>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPriori")));
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPrioriByClient(int ClienteId) {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await GetAsync<RetData<IEnumerable<string>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriByClient"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<PaylessProdPrioriArchM>> SetPaylessProdPrioriFile(IEnumerable<PaylessProdPrioriArchDet> ListUpload, int ClienteId, string Periodo, string codUsr)
        {
            string JsonParams = JsonConvert.SerializeObject(ListUpload);
            return await PostGetAsyncJson<RetData<PaylessProdPrioriArchM>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPaylessProdPrioriFile"), $"?ClienteId={ClienteId}&Periodo={Periodo}&codUsr={codUsr}"), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>> GetPaylessPeriodPrioriFile()
        {            
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriFile")), "");
        }
        public async Task<RetData<IEnumerable<UsuariosExternos>>> GetClients()
        {
            return await GetAsync<RetData<IEnumerable<UsuariosExternos>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClients")));
        }
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>>>> GetPaylessFileDif(int idProdArch)
        {
            return await GetAsync<RetData<Tuple<IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessFileDif"), $"?idProdArch={idProdArch}"));
        }
        /////////////////////////////////////////Account
        public async Task<string> LoginIe(string _User, string _Password)
        {
            UserModel UserO = new UserModel()
            {
                User = _User,
                Password = _Password
            };
            string JsonParams = JsonConvert.SerializeObject(UserO);
            return await PostAsyncJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/LoginIe")), JsonParams);
        }
        public async Task<RetData<IEnumerable<IenetUsers>>> GetUsers(string HashId)
        {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<IenetUsers>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetUsers")), JsonParams);
        }
        public async Task<RetData<IEnumerable<IenetGroups>>> GetGroups(string HashId)
        {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<IenetGroups>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetGroups")), JsonParams);
        }
        public async Task<RetData<IEnumerable<IenetAccesses>>> GetIenetAccesses(string HashId)
        {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<IenetAccesses>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetIenetAccesses")), JsonParams);
        }
        public async Task<RetData<IEnumerable<IenetGroupsAccesses>>> GetIEnetGroupsAccesses(string HashId)
        {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<IenetGroupsAccesses>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetIEnetGroupsAccesses")), JsonParams);
        }
        public async Task<RetData<IEnumerable<Clientes>>> GetAllClients(string HashId)
        {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<Clientes>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetAllClients")), JsonParams);
        }
        public async Task<RetData<IEnumerable<PaylessTiendas>>> GetAllPaylessStores(string HashId) {
            string JsonParams = JsonConvert.SerializeObject(HashId);
            return await PostGetAsyncJson<RetData<IEnumerable<PaylessTiendas>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetAllPaylessStores")), JsonParams);
        }
        /////////////////////////////////////////
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
