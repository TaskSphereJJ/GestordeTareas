using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class TareaFinalizada
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("ElegirTarea")]
        [Display(Name = "Elegir Tarea")]
        public int IdElegirTarea { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de finalizacion")]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; } = string.Empty;
    }
}