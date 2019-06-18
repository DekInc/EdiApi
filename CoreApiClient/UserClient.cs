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
        public async Task<RetData<PaylessTiendas>> GetClient(int TiendaId)
        {
            string JsonParams = JsonConvert.SerializeObject(TiendaId);
            string JsonRes = await PostAsyncJson<string>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClient")), JsonParams);
            return JsonConvert.DeserializeObject<RetData<PaylessTiendas>>(JsonRes);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosByTienda(int ClienteId, int TiendaId)
        {
            return await GetAsync<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosByTienda"), $"?ClienteId={ClienteId}&TiendaId={TiendaId}"));
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosMDetAdmin(int ClienteId) {
            return await GetAsync<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternos"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<IEnumerable<PaylessProdPrioriDetModel>>> GetPedidosExternosDet(int PedidoId) {
            string JsonParams = JsonConvert.SerializeObject(PedidoId);
            return await PostGetAsyncJson<RetData<IEnumerable<PaylessProdPrioriDetModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosDet")), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosGuardados(int ClienteId) {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosGuardados")), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosPendientes(int ClienteId) {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosPendientes")), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>> GetPedidosExternosPendientesByTienda(int ClienteId, int TiendaId) {
            return await GetAsync<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosPendientesByTienda"), $"?ClienteId={ClienteId}&TiendaId={TiendaId}"));
        }
        public async Task<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>> GetPedidosExternosPendientesAdmin()
        {
            return await GetAsync<RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosExternosPendientesAdmin")));
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
        public async Task<RetData<PedidosExternos>> SetPedidoExterno(IEnumerable<PaylessProdPrioriDetModel> ListDis, int ClienteId, int IdEstado, string cboPeriod)
        {            
            string JsonParams = JsonConvert.SerializeObject(ListDis);
            return await PostGetAsyncJson<RetData<PedidosExternos>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPedidoExterno"), $"?ClienteId={ClienteId}&IdEstado={IdEstado}&cboPeriod={cboPeriod}"), JsonParams);
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
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetWmsGroupDispatchs(int ClienteId) {
            return await GetAsync<RetData<IEnumerable<PedidosWmsModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsGroupDispatchs"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetWmsGroupDispatchsBills(int ClienteId) {
            return await GetAsync<RetData<IEnumerable<PedidosWmsModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsGroupDispatchsBills"), $"?ClienteId={ClienteId}"));
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
        public async Task<RetData<IEnumerable<PaylessProdPrioriDetModel>>> GetPaylessProdPriori(string Period)
        {
            string JsonParams = JsonConvert.SerializeObject(Period);
            return await PostGetAsyncJson<RetData<IEnumerable<PaylessProdPrioriDetModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessProdPriori")), JsonParams);
        }
        public async Task<RetData<IEnumerable<PaylessProdPrioriDetModel>>> GetPaylessProdPrioriAll(string TiendaId) {
            return await PostGetAsyncJson<RetData<IEnumerable<PaylessProdPrioriDetModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessProdPrioriAll"), $"?TiendaId={TiendaId}"), "");
        }
        public async Task<RetData<IEnumerable<PaylessProdPrioriDetModel>>> GetPaylessProdPrioriAll() {
            return await GetAsync<RetData<IEnumerable<PaylessProdPrioriDetModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessProdPrioriAll")));
        }
        public async Task<RetData<string>> SetPaylessProdPriori(IEnumerable<PaylessUploadFileModel> ListUpload, int ClienteId, string Periodo, string codUsr, string transporte, bool ChkUpDelete)
        {
            string JsonParams = JsonConvert.SerializeObject(ListUpload);
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPaylessProdPriori"), $"?ClienteId={ClienteId}&Periodo={Periodo}&codUsr={codUsr}&transporte={transporte}&ChkUpDelete={ChkUpDelete}"), JsonParams);
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPriori()
        {
            return await GetAsync<RetData<IEnumerable<string>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPriori")));
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPrioriByClient(int ClienteId) {
            string JsonParams = JsonConvert.SerializeObject(ClienteId);
            return await GetAsync<RetData<IEnumerable<string>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriByClient"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<PaylessProdPrioriArchM>> SetPaylessProdPrioriFile(IEnumerable<PaylessProdPrioriArchDet> ListUpload, int IdTransporte, string Periodo, string codUsr)
        {
            string JsonParams = JsonConvert.SerializeObject(ListUpload);
            return await PostGetAsyncJson<RetData<PaylessProdPrioriArchM>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetPaylessProdPrioriFile"), $"?IdTransporte={IdTransporte}&Periodo={Periodo}&codUsr={codUsr}"), JsonParams);
        }
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>> GetPaylessPeriodPrioriFile()
        {            
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriFile")), "");
        }
        public async Task<RetData<IEnumerable<Clientes>>> GetClients()
        {
            return await GetAsync<RetData<IEnumerable<Clientes>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClients")));
        }
        public async Task<RetData<string>> GetClientById(int ClienteId) {
            return await GetAsync<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetClientById"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<IEnumerable<PaylessProdPrioriDetModel>>> GetPaylessFileDif(string idProdArch, int idData)
        {
            return await GetAsync<RetData<IEnumerable<PaylessProdPrioriDetModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessFileDif"), $"?idProdArch={idProdArch}&idData={idData}"));
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
        public async Task<RetData<Tuple<IEnumerable<IenetGroups>, IEnumerable<IenetAccesses>, IEnumerable<IenetGroupsAccesses>, IEnumerable<IenetGroupsAccesses>>>> GetLoginStruct(string IdGroup) {
            return await GetAsync<RetData<Tuple<IEnumerable<IenetGroups>, IEnumerable<IenetAccesses>, IEnumerable<IenetGroupsAccesses>, IEnumerable<IenetGroupsAccesses>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetLoginStruct"), $"?IdGroup={IdGroup}"));
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
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>>> GetPaylessPeriodPrioriFileExists(string Period, int ClienteId) {
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriFileExists"), $"?Period={Period}&ClienteId={ClienteId}"), "");
        }
        public async Task<RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>>> GetPaylessPeriodPrioriFileExists2(string TiendaId) {
            return await PostGetAsyncJson<RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessPeriodPrioriFileExists2"), $"?TiendaId={TiendaId}"), "");
        }
        public async Task<RetData<bool>> ChangePedidoState(int PedidoId, int ClienteId) {
            return await PostGetAsyncJson<RetData<bool>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/ChangePedidoState"), $"?PedidoId={PedidoId}&ClienteId={ClienteId}"), "");
        }
        public async Task<RetData<IEnumerable<PaylessReportes>>> GetPaylessReportes() {
            return await GetAsync<RetData<IEnumerable<PaylessReportes>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPaylessReportes")));
        }
        public async Task<RetData<IEnumerable<WmsFileModel>>> GetWmsFile(string Period, int IdTransport) {
            return await GetAsync<RetData<IEnumerable<WmsFileModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsFile"), $"?Period={Period}&IdTransport={IdTransport}"));
        }
        public async Task<RetData<string>> SetGroupAccess(int IdGroup, int IdAccess) {
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetGroupAccess"), $"?IdGroup={IdGroup}&IdAccess={IdAccess}"), "");
        }
        public async Task<RetData<IEnumerable<PaylessPeriodoTransporteModel>>> GetTransportByPeriod(string Period) {
            return await PostGetAsyncJson<RetData<IEnumerable<PaylessPeriodoTransporteModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetTransportByPeriod"), $"?Period={Period}"), "");
        }
        public async Task<RetData<IEnumerable<Bodegas>>> GetWmsBodegas(int LocationId) {
            return await PostGetAsyncJson<RetData<IEnumerable<Bodegas>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsBodegas"), $"?LocationId={LocationId}"), "");
        }
        public async Task<RetData<IEnumerable<Regimen>>> GetWmsRegimen(int BodegaId) {
            return await PostGetAsyncJson<RetData<IEnumerable<Regimen>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsRegimen"), $"?BodegaId={BodegaId}"), "");
        }
        public async Task<RetData<string>> SetIngresoExcelWms2(IEnumerable<WmsFileModel> ListProducts, int cboBodega, int cboRegimen) {
            string JsonParams = JsonConvert.SerializeObject(ListProducts);
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetIngresoExcelWms2"), $"?cboBodega={cboBodega}&cboRegimen={cboRegimen}"), JsonParams);
        }
        public async Task<RetData<string>> SetSalidaWmsFromEscaner(IEnumerable<string> ListProducts2, string dtpPeriodo, int cboBodegas, int cboRegimen) {
            string JsonParams = JsonConvert.SerializeObject(ListProducts2);
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetSalidaWmsFromEscaner"), $"?dtpPeriodo={dtpPeriodo}&cboBodegas={cboBodegas}&cboRegimen={cboRegimen}"), JsonParams);
        }
        public async Task<RetData<string>> SetNewDisPayless(string dtpFechaEntrega, int txtWomanQty, int txtManQty, int txtKidQty, int txtAccQty, string radInvType, int ClienteId, int TiendaId, bool? Divert, bool? FullPed, int? TiendaIdDestino) {
            string JsonParams = JsonConvert.SerializeObject(TiendaId);
            return await PostGetAsyncJson<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/SetNewDisPayless"), $"?dtpFechaEntrega={dtpFechaEntrega}&txtWomanQty={txtWomanQty}&txtManQty={txtManQty}&txtKidQty={txtKidQty}&txtAccQty={txtAccQty}&radInvType={radInvType}&ClienteId={ClienteId}&TiendaId={TiendaId}&Divert={Divert}&FullPed={FullPed}&TiendaIdDestino={TiendaIdDestino}"), JsonParams);
        }
        public async Task<RetData<IEnumerable<PaylessTiendas>>> GetStores() {
            return await GetAsync<RetData<IEnumerable<PaylessTiendas>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetStores")));
        }
        public async Task<RetData<IEnumerable<AsyncStates>>> GetAsyncState(int Typ) {
            return await GetAsync<RetData<IEnumerable<AsyncStates>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetAsyncState"), $"?Typ={Typ}"));
        }
        public async Task<RetData<IEnumerable<FE830DataAux>>> GetStockByTienda(int ClienteId, int TiendaId) {
            return await GetAsync<RetData<IEnumerable<FE830DataAux>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetStockByTienda"), $"?ClienteId={ClienteId}&TiendaId={TiendaId}"));
        }
        public async Task<RetData<IEnumerable<FE830DataAux>>> GetStockByCliente(int ClienteId) {
            return await GetAsync<RetData<IEnumerable<FE830DataAux>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetStockByClient"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<List<PedidosPendientesAdmin>>> GetPedidosPendientesAdmin() {
            return await GetAsync<RetData<List<PedidosPendientesAdmin>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosPendientesAdmin")));
        }
        public async Task<RetData<string>> ChangeUserClient(int IdUser, int ClienteId) {
            return await GetAsync<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/ChangeUserClient"), $"?IdUser={IdUser}&ClienteId={ClienteId}"));
        }
        public async Task<RetData<string>> ChangeUserTienda(int IdUser, int TiendaId) {
            return await GetAsync<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/ChangeUserTienda"), $"?IdUser={IdUser}&TiendaId={TiendaId}"));
        }
        public async Task<RetData<IEnumerable<PeticionesAdminBGModel>>> GetPeticionesAdminB() {
            return await GetAsync<RetData<IEnumerable<PeticionesAdminBGModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPeticionesAdminB")));
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetPedidosMWmsByTienda(int ClienteId, int TiendaId) {
            return await GetAsync<RetData<IEnumerable<PedidosWmsModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetPedidosMWmsByTienda"), $"?ClienteId={ClienteId}&TiendaId={TiendaId}"));
        }
        public async Task<RetData<string>> ChangePedidoExternoIdWMS(int PedidoId, int PedidoIdWms) {
            return await GetAsync<RetData<string>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/ChangePedidoExternoIdWMS"), $"?PedidoId={PedidoId}&PedidoIdWms={PedidoIdWms}"));
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetWmsDetDispatchsBills(int ClienteId) {
            return await GetAsync<RetData<IEnumerable<PedidosWmsModel>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWmsDetDispatchsBills"), $"?ClienteId={ClienteId}"));
        }
        public async Task<RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>>> GetWeekReport(int Id, string Typ) {
            return await GetAsync<RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>>>(CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Data/GetWeekReport"), $"?Id={Id}&Typ={Typ}"));
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
