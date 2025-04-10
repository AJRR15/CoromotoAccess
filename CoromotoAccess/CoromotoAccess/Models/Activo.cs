using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Activo
    {
        public int IdActivo { get; set; }
        public int IdHabitacion { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }

        public string CategoriaNombre { get; set; }
    }

}