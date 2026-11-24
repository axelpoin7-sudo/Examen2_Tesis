using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examen2.Models
{
    public class Tesis
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(250)]
        public string Titulo { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Estudiante { get; set; } = string.Empty;

        [Required, MaxLength(80)]
        public string Estado { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Imagen { get; set; } = string.Empty;

        public Guid IdTutor { get; set; }
        [ForeignKey("IdTutor")]
        public Tutor? Tutor { get; set; }
    }
}