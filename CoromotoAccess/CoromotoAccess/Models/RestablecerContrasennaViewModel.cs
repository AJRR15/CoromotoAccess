using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CoromotoAccess.Models
{
    public class RestablecerContrasennaViewModel
    {
        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [Display(Name = "Nueva Contraseña")]
        public string NuevaContrasenna { get; set; }

        [Required(ErrorMessage = "La confirmación es obligatoria")]
        [Compare("NuevaContrasenna", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmarContrasenna { get; set; }

        public string Token { get; set; }
    }
}