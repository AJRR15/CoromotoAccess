using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Usuario
    {
        public long ConsecutivoCliente { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string FotoPerfil { get; set; }
        public string Contrasenna { get; set; }
        public int ConsecutivoRol { get; set; }
        public bool Activo { get; set; }
        public bool TieneContrasennaTemp { get; set; }
        public DateTime? FechaVencimientoTemp { get; set; }
    }
}