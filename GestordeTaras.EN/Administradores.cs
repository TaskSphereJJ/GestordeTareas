using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class Administradores
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Usuarios")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Usuarioo")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "maximo 100 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; } = string.Empty;
    }
}
