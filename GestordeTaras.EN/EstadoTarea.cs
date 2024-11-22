using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GestordeTaras.EN
{
    public class EstadoTarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(50, ErrorMessage = "Maximo 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}
