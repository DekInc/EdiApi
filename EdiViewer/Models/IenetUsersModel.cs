using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models
{
    public class IenetUsersModel
    {
        public int Id { get; set; }
        public int recid { get { return Id; } }
        public int IdIenetGroup { get; set; }
        public string IenetGroup { get; set; }
        public string CodUsr { get; set; }
        public string NomUsr { get; set; }
        public int? ClienteId { get; set; }
        public string Cliente { get; set; }
    }
}
