using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class Proyecto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MaxLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]

        [Display(Name = "Fecha de finalizacion")]
        [DataType(DataType.Date)]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Usuario")]
        [Display(Name = "Admin")]
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public List<Tarea> Tareas { get; set; } // Propiedad de navegación

    }
}

