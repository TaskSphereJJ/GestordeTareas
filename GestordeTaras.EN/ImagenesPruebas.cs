using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class ImagenesPruebas
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "La Ruta de la Imagen es Requerida")]
        [MaxLength(4000, ErrorMessage = "Máximo 4000 Caractéres")]
        [Display(Name = "RutaImagen")]
        public string Imagen { get; set; } = string.Empty;


        [ForeignKey("TareaFinalizada")]
        [Required(ErrorMessage = "Tarea finalizada es Requerida")]
        [Display(Name = "Estado de la Tarea")]
        public int IdTareaFinalizada { get; set; }


        
    }
}
