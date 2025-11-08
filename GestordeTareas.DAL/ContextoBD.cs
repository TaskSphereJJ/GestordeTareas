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
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<EstadoTarea> EstadoTarea { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<ImagenesPrueba> ImagenesPrueba { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<ElegirTarea> ElegirTarea { get; set; }
        public DbSet<TareaFinalizada> TareaFinalizada { get; set; }
        public DbSet<ProyectoUsuario> ProyectoUsuario { get; set; }
        public DbSet<InvitacionProyecto> InvitacionProyecto { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCode { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            optionsbuilder.UseSqlServer(@"data source = desktop-l1nkjnu\sqlexpress; initial catalog = GestordeTareasBD; integrated security = true; encrypt = false; trustservercertificate =true");

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=SQL1003.site4now.net;Initial Catalog=db_ab72aa_gestordetareas;User Id=db_ab72aa_gestordetareas_admin; 
        //                          Password=gestor123456;
        //                          Encrypt=True; 
        //                          TrustServerCertificate=True;");
        //}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvitacionProyecto>()
                .HasOne(i => i.Usuario)  // Relación con Usuario
                .WithMany()  // Usuario puede tener muchas invitaciones
                .HasForeignKey(i => i.IdUsuario)  // Clave foránea en InvitacionProyecto
                .OnDelete(DeleteBehavior.Cascade);  // Eliminar invitaciones asociadas cuando se elimina el usuario


        }

    }
}