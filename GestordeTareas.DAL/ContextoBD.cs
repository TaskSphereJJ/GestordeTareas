using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GestordeTareas.DAL
{
    public class ContextoBD : DbContext
    {
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Prioridad> Prioridad { get; set; }
//<<<<<<< HEAD
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<IniciarSesionEN> IniciarSesionEN { get; set; }
        public DbSet<EstadoTareaEN> EstadoTareaEN { get; set; }


//=======
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenTarea> ImagenTarea { get; set; }
        public DbSet<AsignacionTareas> AsignacionTareas { get; set; }
//>>>>>>> 36c183c47eae96d622aa318fc0e65251551feb9f
    }
}
