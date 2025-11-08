using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class PasswordResetCode
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Expiration { get; set; }
        [ForeignKey("Usuario")]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

    }

}

