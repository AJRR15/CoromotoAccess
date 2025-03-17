using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Nomina
    {
        public int IdNomina { get; set; }
        public int IdEmpleado { get; set; }
        public decimal Salario { get; set; }
        public decimal Bono { get; set; }
        public decimal Multa { get; set; }
        public decimal Total { get; set; }
    }
}