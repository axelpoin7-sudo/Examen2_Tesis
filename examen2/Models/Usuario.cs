using System.ComponentModel.DataAnnotations;

namespace examen2.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}