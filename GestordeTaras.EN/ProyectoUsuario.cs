using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class ProyectoUsuario
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Encargado { get; set; }
        [ForeignKey("Proyecto")]
        [Display(Name = "Usuario")]
        public int IdProyecto { get; set; }
        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }
        public Proyecto Proyecto { get; set; }
        public Usuario Usuario { get; set; }

    }
}