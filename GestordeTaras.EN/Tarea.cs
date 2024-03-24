using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestordeTaras.EN;

namespace GestordeTaras.EN
{
    public class Tarea
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = " El Nombre es Requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 Caractéres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Escriba la Descripción")]
        [MaxLength(600, ErrorMessage = "Máximo 600 caracteres")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Fecha de creacion es Requerida")]
        [Display(Name = "Fecha  de creacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La Fecha de Vencimiento es Requerida")]
        [Display(Name = "Fecha  de vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [ForeignKey("Categoria")]
        [Required(ErrorMessage = " La Categoría es Requerida")]
        [Display(Name = "Categoria")]
        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; } // Propiedad de navegación

        [ForeignKey("Prioridad")]
        [Required(ErrorMessage = "La Prioridad es Requerida")]
        [Display(Name = "Prioridad")]
        public int IdPrioridad { get; set; }
        public Prioridad Prioridad { get; set; } // Propiedad de navegación

        [ForeignKey("EstadoTarea")]
        [Required(ErrorMessage = "El Estado es Requerido")]
        [Display(Name = "Estado")]
        public int IdEstadoTarea { get; set; }
        public EstadoTarea EstadoTarea { get; set; } // Propiedad de navegación

        [ForeignKey("Proyecto")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
        public Proyecto Proyecto { get; set; } // Propiedad de navegación
    }


}

