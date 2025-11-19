namespace Examenes.Models
{
    
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Boolean Finzalizado { get; set; }

        public int MateriaId { get; set; }
        public virtual Materia Materia { get; set; }

        public int ProfesorId { get; set; }
        public virtual Profesor Profesor { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; }

        public virtual ICollection<Examen> Examenes { get; set; }
    }


    public class Examen
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public double PorcentajeParaAprobacion { get; set; }
        public TipoExamen Tipo { get; set; }

        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }

        public virtual ICollection<Pregunta> Preguntas { get; set; }

        public virtual ICollection<ExamenAlumno> ExamenesAlumnos { get; set; }

        public bool Publicado { get; set; } = false;
    }

    public class Pregunta
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }

        public int ExamenId { get; set; }
        public virtual Examen Examen { get; set; }

        public virtual ICollection<Opcion> Opciones { get; set; }
    }

    public class Opcion
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public bool EsCorrecta { get; set; }

        public int PreguntaId { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }

    public class ExamenAlumno
    {
        public int Id { get; set; }
        public double? Nota { get; set; }
        public int? CantRespCorrectas { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public EstadoExamen Estado { get; set; }

        public int AlumnoId { get; set; }
        public virtual Alumno Alumno { get; set; }

        public int ExamenId { get; set; }
        public virtual Examen Examen { get; set; }

        public virtual ICollection<RespuestaAlumno> Respuestas { get; set; }
    }

    public class RespuestaAlumno
    {
        public int Id { get; set; }

        public int ExamenAlumnoId { get; set; }
        public virtual ExamenAlumno ExamenAlumno { get; set; }

        public int OpcionId { get; set; }
        public virtual Opcion Opcion { get; set; }
    }

    public enum TipoExamen
    {
        PARCIAL,
        FINAL
    }

    public enum EstadoExamen
    {
        PENDIENTE,
        APROBADO,
        DESAPROBADO,
        AUSENTE
    }
}