using GestordeTareas.EN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestordeTaras.EN
{
    public class TareaFinalizada
    {
        [Key]
        public int Id { get; set; }

        // Fecha de finalización: Asignar automáticamente la fecha actual si no se proporciona una.
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFinalizacion { get; set; } = DateTime.Now;

        // Comentarios asociados a la tarea finalizada
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Comentarios")]
        public string Comentarios { get; set; } = string.Empty;

        // Relación con el Estado de la Tarea
        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("EstadoTarea")]
        [Display(Name = "Estado de la Tarea")]
        public int IdEstadoTarea { get; set; }

        // Relación con la Tarea
        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Tarea")]
        [Display(Name = "idTarea")]
        public int IdTarea { get; set; }

        public ICollection<ImagenesPrueba> ImagenesTarea { get; set; } = new List<ImagenesPrueba>();
        public virtual EstadoTarea EstadoTarea { get; set; }
        public virtual Tarea Tarea { get; set; }
    }
}
