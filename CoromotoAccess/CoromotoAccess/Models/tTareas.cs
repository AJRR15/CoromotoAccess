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
    
    public partial class tTareas
    {
        public int IdTarea { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public int IdEmpleado { get; set; }
    
        public virtual tEmpleados tEmpleados { get; set; }
    }
}
