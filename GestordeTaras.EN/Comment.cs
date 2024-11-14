using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("Proyecto")]
        public int IdProyecto { get; set; }

        //[Column(TypeName = "datetime2")]
        public DateTime FechaComentario { get; set; }

        [NotMapped]
        public Usuario Usuario { get; set; } = new Usuario();

        [NotMapped]
        public Proyecto Proyecto { get; set; } = new Proyecto();

    }
}
