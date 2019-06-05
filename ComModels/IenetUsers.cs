using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class IenetUsers
    {
        public int Id { get; set; }
        public int IdIenetGroup { get; set; }
        public string CodUsr { get; set; }
        public string NomUsr { get; set; }
        public string UsrPassword { get; set; }
        public int? ClienteId { get; set; }
        public string HashId { get; set; }
        public int? TiendaId { get; set; }
    }
}
