using api_econsulta.Models;
using Microsoft.EntityFrameworkCore;

namespace api_econsulta.DB
{
    // AppDbContext.cs
public class EconsultaDbContext : DbContext
    {
        public EconsultaDbContext(DbContextOptions<EconsultaDbContext> options) 
            : base(options) 
        { 
        }

        // DbSets para suas entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        // Adicione outros DbSets conforme necessário

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do modelo de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Type).IsRequired();
                
                // Índice único para email
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Configuração do modelo de Schedule
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(s => s.Id);
                
                // Relacionamento com Doctor
                entity.HasOne(s => s.Doctor)
                    .WithMany()
                    .HasForeignKey(s => s.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Relacionamento com Patient
                entity.HasOne(s => s.Patient)
                    .WithMany()
                    .HasForeignKey(s => s.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Índices para melhor performance
                entity.HasIndex(s => new { s.DoctorId, s.StartTime });
                entity.HasIndex(s => new { s.PatientId, s.StartTime });
            });

            // Outras configurações do modelo...
        }
    }
}