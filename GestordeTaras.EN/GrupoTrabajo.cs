using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class GrupoTrabajo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Administradores")]
        [Display(Name = "Admin")]
        public int AdministradorID { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Colaboradores")]
        [Display(Name = "Colaborador")]
        public int ColaboradorID { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("Proyecto")]
        [Display(Name = "Proyecto")]
        public int ProyectoID { get; set; }
    }
}