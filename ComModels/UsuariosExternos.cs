using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class UsuariosExternos
    {
        public int Id { get; set; }
        public string CodUsr { get; set; }
        public string NomUsr { get; set; }
        public string UsrPassword { get; set; }
        public int? ClienteId { get; set; }
        public string HashId { get; set; }
    }
}
