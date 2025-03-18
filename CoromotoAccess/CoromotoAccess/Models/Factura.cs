using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Factura
    {
        public long IdFactura { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaEmision { get; set; }
        public string imagen { get; set; }
        public string total { get; set; }
        public bool Estado { get; set; }
        public string CedulaCliente { get; set; }


    }
}