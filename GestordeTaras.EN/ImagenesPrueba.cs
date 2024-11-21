using GestordeTaras.EN;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestordeTareas.EN
{
    public class ImagenesPrueba
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Ruta de la Imagen es Requerida")]
        [MaxLength(4000, ErrorMessage = "Máximo 4000 Carácteres")]
        [Display(Name = "Ruta de la Imagen")]
        public string Imagen { get; set; } = string.Empty; // Ruta de la imagen

        // Relación con Tarea
        [ForeignKey("Tarea")]
        [Required(ErrorMessage = "La tarea asociada es requerida.")]
        [Display(Name = "Tarea Asociada")]
        public int IdTareaFinalizada { get; set; }

        // Propiedad de navegación
        public virtual TareaFinalizada TareaFinalizada { get; set; }

        [NotMapped]
        public int Top_Aux { get; set; }
    }
}
