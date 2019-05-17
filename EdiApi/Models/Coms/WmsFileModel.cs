using System;
using System.Collections.Generic;
using System.Text;

namespace EdiApi.Models {
    public class WmsFileModel {
        public int Identificador { get; set; }
        public string Fecha { get; set; }
        public string ReciboAlmacen { get; set; }
        public string Barcode { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public int Piezas { get; set; }
        public int Unidad { get; set; }
        public int Cantidad { get; set; }
        public string CodigoLocalizacion { get; set; }
        public double? Peso { get; set; }
        public double? Volumen { get; set; }
        public double? ValorUnitario { get; set; }
        public double? Valor { get; set; }
        public double? NumeroEntrada { get; set; }
        public string Observaciones { get; set; }
        public int OrdenDeCompra { get; set; }
        public string Lote { get; set; }
        public string NumeroFactura { get; set; }
        public string Cliente { get; set; }
        public int RackId { get; set; }
        public string FechaIm5 { get; set; }
        public int Embalaje { get; set; }
        public int UOM { get; set; }
        public int Exportador { get; set; }
        public int Destino { get; set; }
        public string Estilo { get; set; }
        public string CodEquivalente { get; set; }
        public int PaisOrigen { get; set; }
        public string Color { get; set; }
        public string Cp { get; set; }
        public int Cont { get; set; }
        public int ClienteId { get; set; }
        public string Transporte { get; set; }
    }
}
