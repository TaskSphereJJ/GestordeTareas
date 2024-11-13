using GestordeTaras.EN;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuNamespace
{
    public class ElegirTarea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Tarea")]
        public int IdTarea { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El proyecto es requerido")]
        [ForeignKey("Proyecto")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; } // Propiedad de navegación para Usuario
        public virtual Proyecto Proyecto { get; set; } // Propiedad de navegación para Proyecto
        public virtual Tarea Tarea { get; set; } // Propiedad de navegación para Tarea
    }
}