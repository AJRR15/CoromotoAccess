using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Reserva
    {
        public long IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdHabitacion { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public String Comentario { get; set; }
        public int PersonasHospedados { get; set; }
        public bool Estado { get; set; }
        public int IdMoneda { get; set; }
        public int IdMetodoP { get; set; }
    }
}
