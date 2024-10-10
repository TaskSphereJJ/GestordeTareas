using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestordeTaras.EN
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Correo Electrónico")]
        public string NombreUsuario { get; set; } = string.Empty;

        // Hacemos que el campo Pass sea opcional (nullable)
        [Display(Name = "Contraseña")]
        public string? Pass { get; set; } // Ahora puede ser null para usuarios autenticados con Google

        [NotMapped] // Esto es importante para que no se mapee en la base de datos
        public string ConfirmarPass { get; set; }

        [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public byte Status { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }

        [ForeignKey("Cargo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar

        // Este campo se requiere para las contraseñas de usuarios no autenticados con Google
        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres", MinimumLength = 6)]
        [Compare("Pass", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar la contraseña")]
        public string ConfirmPassword_Aux { get; set; } = string.Empty; // propiedad auxiliar

        public Cargo Cargo { get; set; } // propiedad de navegación
        public virtual ICollection<ProyectoUsuario> ProyectoUsuario { get; set; }

        // Nuevos campos para autenticación externa (Google, Facebook, etc.)

        [MaxLength]
        public string? Provider { get; set; } // Proveedor externo (Google, Facebook, etc.)

        [MaxLength]
        public string? ProviderKey { get; set; } // Identificador único del usuario en el proveedor externo
    }

    public enum User_Status
    {
        ACTIVO = 1, INACTIVO = 2
    }
}
