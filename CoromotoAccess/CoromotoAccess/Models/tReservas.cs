//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoromotoAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tReservas
    {
        public int IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdHabitacion { get; set; }
        public System.DateTime CheckIn { get; set; }
        public System.DateTime CheckOut { get; set; }
        public int PersonasHospedadas { get; set; }
        public string Comentario { get; set; }
        public bool Estado { get; set; }
        public int IdMoneda { get; set; }
        public int IdMetodoP { get; set; }
    
        public virtual tHabitaciones tHabitaciones { get; set; }
        public virtual tMetodoPago tMetodoPago { get; set; }
        public virtual tMonedas tMonedas { get; set; }
        public virtual tUsuario tUsuario { get; set; }
    }
}
