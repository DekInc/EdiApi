using EdiApi.Models.EdiDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public class WmsInOutGModel : WmsInOut {
        public int Recid { get { return Id; } }
        public string FechaTran { get { return FechaTransaccion.Value.ToString(ApplicationSettings.DateTimeFormat); } }
    }
}
