using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " El comentario es Requerido")]
        [Display(Name = "Comentario")] 
        public string Content { get; set; }
        [Required(ErrorMessage = "La Fecha de creacion es Requerida")]
        [Display(Name = "Fecha  de comentario")]
        public DateTime FechaComentario { get; set; } = DateTime.Now;

        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("Proyecto")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
        public Proyecto Proyecto { get; set; } 
        public Usuario Usuario { get; set; } 

    }
}