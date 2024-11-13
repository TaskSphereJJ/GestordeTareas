using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuNamespace;

namespace GestordeTaras.EN
{
    public class TareaFinalizada
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("ElegirTarea")]
        [Display(Name = "Elegir Tarea")]
        public int IdElegirTarea { get; set; }

        // Relación con las imágenes de la tarea finalizada
        public ICollection<ImagenesPrueba> Imagenes { get; set; } = new List<ImagenesPrueba>();
        public virtual ElegirTarea ElegirTarea { get; set; }

    }
}
