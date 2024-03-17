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
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<EstadoTareaEN> EstadoTarea { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenesPruebas> ImagenesPruebas { get; set; }
        public DbSet<AsignacionTareas> AsignacionTareas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source = DESKTOP-K6HMCBE; Initial Catalog = GestorTareas; Integrated Security = True; Encrypt = false; trustServerCertificate =true");
        }

    }
}
