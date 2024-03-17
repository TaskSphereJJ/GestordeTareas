using GestordeTaras.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuNamespace;
using static System.Net.Mime.MediaTypeNames;

namespace GestordeTareas.DAL
{
    public class ContextoBD : DbContext
    {
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Prioridad> Prioridad { get; set; }
<<<<<<< HEAD
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<EstadoTarea> EstadoTarea { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenesPrueba> ImagenesPrueba { get; set; }
        //public DbSet<AsignacionTareas> AsignacionTareas { get; set; }
=======
        public DbSet<Usuarios> Usuario { get; set; }
        public DbSet<EstadoTareaEN> EstadoTarea { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenesPruebas> ImagenesPruebas { get; set; }
>>>>>>> dff9044fdaea6a59a1e888c274b2ff2395000048
        public DbSet<Proyecto> Proyecto { get; set; }

        public DbSet<ElegirTarea> ElegirTarea { get; set ; }
        public DbSet<TareaFinalizada> TareaFinalizada { get; set ; }

           protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source = DESKTOP-UMST7PO; Initial Catalog = GestordeTareasBD; Integrated Security = True; Encrypt = false; trustServerCertificate =true");
        }

    }
}
