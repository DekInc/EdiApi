using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class BarcodeProducto
    {
        public int BarcodeId { get; set; }
        public string Barcode { get; set; }
        public string CodProducto { get; set; }

        public Producto CodProductoNavigation { get; set; }
    }
}
