using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Evaluacion
    {
        public int IdEvaluacion { get; set; }
        public int IdEmpleado { get; set; }
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
        public DateTime? FechaEvaluacion { get; set; }

        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public string RolEmpleado { get; set; }
        public int HorasTrabajadas { get; set; }
    }
}