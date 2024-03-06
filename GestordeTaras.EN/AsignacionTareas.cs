using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class AsignacionTareas
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Fecha de Asignacion es Requerida")]
        [Display(Name = "Fecha  de Asignacion")]
        public DateTime FechaAsignada { get; set; }


        [Required(ErrorMessage = "La Fecha de Finalizacion es Requerida")]
        [Display(Name = "Fecha  de Finalizacion")]
        public DateTime FechaFinalizacion { get; set; }

        [ForeignKey("Tarea")]
        [Required(ErrorMessage = "La Tarea es requerida")]
        [Display(Name = "Estado de la Tarea")]
        public int TareaId { get; set; }


        [ForeignKey("Usuario")]
        [Required(ErrorMessage = "El Usuario es requerida")]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }
    }
}