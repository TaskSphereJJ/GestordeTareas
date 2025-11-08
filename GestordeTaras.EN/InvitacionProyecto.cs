using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class InvitacionProyecto
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Proyecto")]
        [Display(Name = "Proyecto")]
        public int IdProyecto { get; set; }
        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "El token es requerido.")]
        public string Token { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public Usuario Usuario { get; set; }
        public Proyecto Proyecto { get; set; }
    }


}
