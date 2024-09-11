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
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;

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

        [NotMapped]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar la contraseña")]
        public string ConfirmPassword_Aux { get; set; } = string.Empty; // propiedad auxiliar

        public Cargo Cargo { get; set; } // propiedad de navegación
    }
    public enum User_Status
    {
        ACTIVO = 1, INACTIVO = 2
    }

}

