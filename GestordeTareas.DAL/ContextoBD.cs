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
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<EstadoTarea> EstadoTarea { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenesPrueba> ImagenesPrueba { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<ElegirTarea> ElegirTarea { get; set; }
        public DbSet<TareaFinalizada> TareaFinalizada { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=GestorTareasBD;User=Jeffrey;Password=jeffrey20068f;Encrypt=true;TrustServerCertificate=true;");
            
        }
    }
}