using Biblioteca_Examen2.DTOs;

namespace examen2.Interfaces
{
    public interface ITutorInterface
    {
        List<TutorDto> ObtenerTodos();
        void Crear(TutorDto dto);
    }
}