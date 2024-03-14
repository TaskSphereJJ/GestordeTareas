using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class ElegirTarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Tarea")]
        [Display(Name = "Tarea")]
        public int IdTarea { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Colaboradores")]
        [Display(Name = "Colaboradores")]
        public int IdColaborador { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de Asignacion")]
        public DateTime FechaAsignacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("EstadoTarea")]
        [Display(Name = "Estado Tarea")]
        public int IdEstadoTarea { get; set; }
    }
}
