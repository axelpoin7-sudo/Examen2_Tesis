using Biblioteca_Examen2.DTOs;

namespace examen2.Interfaces
{
    public interface ITesisInterface
    {
        List<TesisDto> ObtenerTodas();
        List<TesisDto> Buscar(string? filtro);
        Task CrearAsync(CrearTesisDto dto, IFormFile archivoImagen);
    }
}