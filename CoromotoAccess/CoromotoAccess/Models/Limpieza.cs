using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Limpieza
    {
        public int IdLimpieza { get; set; }
        public int IdEmpleado { get; set; }
        public int IdHabitacion { get; set; }
        public DateTime FechaLimpieza { get; set; }
        public bool Estado { get; set; }
    }
}