using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class ImagenTarea
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "La Ruta de la Imagen es Requerida")]
        [MaxLength(4000, ErrorMessage = "Máximo 4000 Caractéres")]
        [Display(Name = "RutaImagen")]
        public string RutaImagen { get; set; } = string.Empty;


        [ForeignKey("Tarea")]
        [Required(ErrorMessage = "El Estado es Requerido")]
        [Display(Name = "Estado de la Tarea")]
        public int IdTarea { get; set; }

        public int Top_Aux { get; set; }
        public Tarea Tarea { get; set; } = new Tarea();
        
    }
}
