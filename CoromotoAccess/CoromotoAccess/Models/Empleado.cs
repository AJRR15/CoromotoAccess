﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoromotoAccess.Models
{
    public class Empleado
    {
        public int ConsecutivoEmp { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string FotoPerfil { get; set; }
        public string Contrasenna { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }
        public bool TieneContrasennaTemp { get; set; }
        public DateTime FechaVencimientoTemp { get; set; }
        public int IdVilla { get; set; }
        public int IdTurno { get; set; }
        public int Vacaciones { get; set; }
        public int HorasTrabajadas { get; set; }
    }
}