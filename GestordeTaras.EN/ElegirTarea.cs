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
        [ForeignKey("Usuarios")]
        public int IdUsuario { get; set; }

        [ForeignKey("Proyecto")]
        [Required(ErrorMessage = "El proyecto es requerido")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
    }
}