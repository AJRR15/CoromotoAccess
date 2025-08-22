using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Habitacion
    {
        public int IdHabitacion { get; set; }
        public string NombreHabitacion { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio {get; set; }
        public DateTime CheckIn {  get; set; }
        public DateTime CheckOut { get; set; }
        public bool Estado { get; set; }
        public int IdVilla {get; set; } 
        public int IdTipoHabitacion {get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
        public string img5 { get; set; }
        public string img6 { get; set; }

    }
}