using CoromotoAccess.Models;
using System;

namespace CoromotoAccess.Models
{
    public class Turnos
    {
        public int IdTurno { get; set; }
        public string Nombre { get; set; }
        public string HoraInicio { get; set; }
        public string HoraSalida { get; set; }
    }
}