using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Vacaciones
    {
        public int IdVacacion { get; set; }
        public int IdEmpleado { get; set; }
        public string DiasSolicitados { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? Estado { get; set; }
    }
}