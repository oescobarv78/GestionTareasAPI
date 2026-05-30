using Microsoft.EntityFrameworkCore;
using GestionTareasAPI.Models;

namespace GestionTareasAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tarea> Tareas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarea>()
                .Property(t => t.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Tarea>()
                .Property(t => t.Prioridad)
                .HasConversion<string>();

            modelBuilder.Entity<Tarea>()
                .Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
