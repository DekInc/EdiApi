using ComModels.Models.EdiDB;

namespace ComModels {
    public partial class PaylessProdPrioriDetModel : PaylessProdPrioriDet
    {
        public int Recid { get { return Id; } }
        public int Existencia { get; set; }
        public int Reservado { get; set; }
        public int Disponible { get { return Existencia - Reservado; } }
        public int CantPedir { get; set; }
        public string DateProm { set; get; }
        public string Transporte { set; get; }
        public string Tienda { get { return (string.IsNullOrEmpty(Barcode) ? string.Empty : Barcode.Substring(0, 4)); } }
    }
}
