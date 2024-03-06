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
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Maximo 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        [ForeignKey("Cargo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }
    }
}
