using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class Clientes
    {        
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Nit { get; set; }
        public string Nrc { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string DiasPago { get; set; }
        public string Comentario { get; set; }
        public int? EstatusId { get; set; }
        public string Contacto { get; set; }
        public string TelefonoContacto { get; set; }
        public string EmailContacto { get; set; }
        public string OrderEmailNotifica { get; set; }
    }
}
