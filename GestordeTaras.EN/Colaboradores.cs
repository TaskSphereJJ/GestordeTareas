using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class Colaboradores
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Usuarios")]
        [Display(Name = "Usuario")]
        public string UsuarioID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; } = string.Empty;
    }
}