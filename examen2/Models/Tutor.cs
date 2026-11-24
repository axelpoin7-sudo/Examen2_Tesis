using System.ComponentModel.DataAnnotations;

namespace examen2.Models
{
    public class Tutor
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(120)]
        public string Especialidad { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Correo { get; set; } = string.Empty;

        public ICollection<Tesis> Tesis { get; set; } = new List<Tesis>();
    }
}