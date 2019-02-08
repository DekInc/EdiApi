using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class ItemParamaetroxProducto
    {
        public int? InventarioId { get; set; }
        public int? ItemInventarioId { get; set; }
        public string CodProducto { get; set; }
        public int? ParametroId { get; set; }
        public string ValParametro { get; set; }
        public int ItemParamProducto { get; set; }
    }
}
