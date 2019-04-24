using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class PaylessProdPrioriDet
    {
        public int Id { get; set; }
        public int IdPaylessProdPrioriM { get; set; }
        public string Oid { get; set; }
        public string Barcode { get; set; }
        public string Tienda { get { return Barcode.Substring(0, 4); } }
        public string Estado { get; set; }
        public string Pri { get; set; }
        public string PoolP { get; set; }
        public string Producto { get; set; }
        public string Talla { get; set; }
        public string Lote { get; set; }
        public string Categoria { get; set; }
        public string Departamento { get; set; }
        public string Cp { get; set; }
        public string Pickeada { get; set; }
        public string Etiquetada { get; set; }
        public string Preinspeccion { get; set; }
        public string Cargada { get; set; }
        public double? M3 { get; set; }
        public double? Peso { get; set; }
    }

    public partial class PaylessProdPrioriDetModel: PaylessProdPrioriDet
    {
        public int recid { get { return Id; } }
        public int Existencia { get; set; }
        public int Reservado { get; set; }
        public int Disponible { get { return Existencia - Reservado; } }
        public int CantPedir { get; set; }
        public List<PaylessProdPrioriDet> Childrens { get; set; }
    }
}
