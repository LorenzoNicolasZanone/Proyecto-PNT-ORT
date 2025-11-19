using Microsoft.EntityFrameworkCore;
using Examenes.Models;

namespace Examenes.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<ExamenAlumno> ExamenesAlumnos { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Opcion> Opciones { get; set; }
        public DbSet<RespuestaAlumno> RespuestasAlumnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RespuestaAlumno>()
                .HasOne(r => r.Opcion)
                .WithMany()
                .HasForeignKey(r => r.OpcionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}