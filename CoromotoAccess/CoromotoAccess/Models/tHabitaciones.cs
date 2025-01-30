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
    
    public partial class tHabitaciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tHabitaciones()
        {
            this.tActivos = new HashSet<tActivos>();
            this.tLimpiezas = new HashSet<tLimpiezas>();
            this.tReservas = new HashSet<tReservas>();
        }
    
        public int IdHabitacion { get; set; }
        public string NombreHabitacion { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public System.DateTime CheckIn { get; set; }
        public System.DateTime CheckOut { get; set; }
        public bool Estado { get; set; }
        public int IdVilla { get; set; }
        public int IdTipoHabitacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tActivos> tActivos { get; set; }
        public virtual tTiposHabitaciones tTiposHabitaciones { get; set; }
        public virtual tVillas tVillas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tLimpiezas> tLimpiezas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tReservas> tReservas { get; set; }
    }
}
