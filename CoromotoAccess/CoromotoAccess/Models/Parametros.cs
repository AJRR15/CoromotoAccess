using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Parametros
    {
        public List<tMetodoPago> MetodosPago { get; set; }
        public List<tTiposHabitaciones> TiposHabitacion { get; set; }
        public List<tMonedas> TiposMoneda { get; set; }
        public List<tVillas> Villas { get; set; }

        public List<tCategorias> Categorias { get; set; }
    }
}