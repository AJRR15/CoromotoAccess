﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BDCoromotoEntities : DbContext
    {
        public BDCoromotoEntities()
            : base("name=BDCoromotoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tActivos> tActivos { get; set; }
        public virtual DbSet<tCategorias> tCategorias { get; set; }
        public virtual DbSet<tContactenos> tContactenos { get; set; }
        public virtual DbSet<tEmpleados> tEmpleados { get; set; }
        public virtual DbSet<tEvaluaciones> tEvaluaciones { get; set; }
        public virtual DbSet<tFacturas> tFacturas { get; set; }
        public virtual DbSet<tHabitaciones> tHabitaciones { get; set; }
        public virtual DbSet<tLimpiezas> tLimpiezas { get; set; }
        public virtual DbSet<tMetodoPago> tMetodoPago { get; set; }
        public virtual DbSet<tMonedas> tMonedas { get; set; }
        public virtual DbSet<tNominas> tNominas { get; set; }
        public virtual DbSet<tPermisos> tPermisos { get; set; }
        public virtual DbSet<tReservas> tReservas { get; set; }
        public virtual DbSet<tRoles> tRoles { get; set; }
        public virtual DbSet<tTiposHabitaciones> tTiposHabitaciones { get; set; }
        public virtual DbSet<tTurnos> tTurnos { get; set; }
        public virtual DbSet<tUsuario> tUsuario { get; set; }
        public virtual DbSet<tVacaciones> tVacaciones { get; set; }
        public virtual DbSet<tVillas> tVillas { get; set; }
    
        public virtual int RegistroUsuario(string identificacion, string nombre, string apellido, string correoElectronico, string telefono, string fotoPerfil, string contrasenna)
        {
            var identificacionParameter = identificacion != null ?
                new ObjectParameter("Identificacion", identificacion) :
                new ObjectParameter("Identificacion", typeof(string));
    
            var nombreParameter = nombre != null ?
                new ObjectParameter("Nombre", nombre) :
                new ObjectParameter("Nombre", typeof(string));
    
            var apellidoParameter = apellido != null ?
                new ObjectParameter("Apellido", apellido) :
                new ObjectParameter("Apellido", typeof(string));
    
            var correoElectronicoParameter = correoElectronico != null ?
                new ObjectParameter("CorreoElectronico", correoElectronico) :
                new ObjectParameter("CorreoElectronico", typeof(string));
    
            var telefonoParameter = telefono != null ?
                new ObjectParameter("Telefono", telefono) :
                new ObjectParameter("Telefono", typeof(string));
    
            var fotoPerfilParameter = fotoPerfil != null ?
                new ObjectParameter("FotoPerfil", fotoPerfil) :
                new ObjectParameter("FotoPerfil", typeof(string));
    
            var contrasennaParameter = contrasenna != null ?
                new ObjectParameter("Contrasenna", contrasenna) :
                new ObjectParameter("Contrasenna", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RegistroUsuario", identificacionParameter, nombreParameter, apellidoParameter, correoElectronicoParameter, telefonoParameter, fotoPerfilParameter, contrasennaParameter);
        }
    }
}
