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
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("Tarea")]
        public int IdTarea { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        [ForeignKey("Proyecto")]
        [Required(ErrorMessage = "El proyecto es requerido")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
        public Tarea Tarea { get; set; }
        public Usuario Usuario { get; set; }
        public Proyecto Proyecto { get; set; }

    }
}