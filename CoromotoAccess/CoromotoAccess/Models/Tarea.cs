using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;

namespace CoromotoAccess.Models
{
    public class Tarea
    {
        public int IdTarea { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int IdEmpleado { get; set; }
    }
}