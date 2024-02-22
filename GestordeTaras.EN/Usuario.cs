using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Required(ErrorMessage = "La Fecha nacimiento es requerida")]
        public DateTime FechaNacimieno { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(9, ErrorMessage = "Maximo 9 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "maximo 50 caracteres")]
        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "Correo Obligatorio")]
        [MaxLength(50,ErrorMessage = "Maximo 50 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo requerido")]
        [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Pass {  get; set; } = string.Empty;

        [ForeignKey("Cargo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }
    }
}
