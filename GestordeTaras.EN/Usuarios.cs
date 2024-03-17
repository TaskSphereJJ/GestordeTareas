using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestordeTaras.EN
{
    public class Usuarios
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
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(4000, ErrorMessage = "maximo 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Teléfono { get; set; } = string.Empty;        

        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("Cargo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cargo")]
        public int CargoId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "Maximo 50 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public int Status { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime RegistrationDate { get; set; }


        [NotMapped]
        public int Top_Aux { get; set; } // propiedad auxiliar

        public Cargo? Cargo { get; set; }

    }

    public enum User_Status
    {
        ACTIVO = 1, INACTIVO = 2
    }
}
