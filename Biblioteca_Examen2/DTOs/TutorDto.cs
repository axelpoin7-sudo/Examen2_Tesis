using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Examen2.DTOs
{
    public class TutorDto
    {
        [Required(ErrorMessage = "El nombre del tutor es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public string Especialidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        public string Correo { get; set; } = string.Empty;
    }
}