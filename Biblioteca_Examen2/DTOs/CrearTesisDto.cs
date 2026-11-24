using System.ComponentModel.DataAnnotations;

namespace Biblioteca_Examen2.DTOs
{
    public class CrearTesisDto
    {
        [Required(ErrorMessage = "El título de la tesis es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del estudiante es obligatorio")]
        public string Estudiante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "En Proceso";

        [Required(ErrorMessage = "Debe seleccionar un tutor")]
        public string NombreTutor { get; set; } = string.Empty;
    }
}