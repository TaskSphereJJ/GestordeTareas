using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestordeTaras.EN
{
    public class ProyectoUsuario
    {
        public int Id { get; set; }
        public int IdProyecto { get; set; }
        public Proyecto Proyecto { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Encargado { get; set; }


    }
}